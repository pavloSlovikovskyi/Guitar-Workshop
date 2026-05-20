using API.Dtos;
using Application.Common;
using Application.Customers.Commands;
using Application.Customers.Queries;
using Domain.Customers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/customers")]
[Authorize]
public class CustomerController : ControllerBase
{
    private readonly IMediator _mediator;

    public CustomerController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize(Roles = AppRoles.Master)]
    public async Task<IActionResult> Create([FromBody] CreateCustomerRequest request)
    {
        var command = new CreateCustomerCommand(
            request.FirstName,
            request.LastName,
            request.PhoneNumber,
            request.Email
        );

        var result = await _mediator.Send(command);
        if (!result.IsSuccess)
            return BadRequest(new { message = result.Error });

        return CreatedAtAction(nameof(GetById), new { id = result.Value!.Value }, new { id = result.Value.Value });
    }

    [HttpGet("me")]
    [Authorize(Roles = $"{AppRoles.Master},{AppRoles.Customer}")]
    public async Task<IActionResult> GetMe()
    {
        var result = await _mediator.Send(new GetCurrentCustomerQuery());
        if (!result.IsSuccess)
            return NotFound(new { message = result.Error });

        return Ok(CustomerResponse.FromDomainModel(result.Value!));
    }

    [HttpGet("{id:guid}")]
    [Authorize(Roles = $"{AppRoles.Master},{AppRoles.Customer}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = new GetCustomerByIdQuery(new CustomerId(id));
        var result = await _mediator.Send(query);
        if (!result.IsSuccess)
            return NotFound(new { message = result.Error });

        return Ok(CustomerResponse.FromDomainModel(result.Value!));
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = $"{AppRoles.Master},{AppRoles.Customer}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCustomerRequest request)
    {
        var command = new UpdateCustomerCommand(
            new CustomerId(id),
            request.FirstName,
            request.LastName,
            request.PhoneNumber,
            request.Email
        );

        var result = await _mediator.Send(command);
        if (!result.IsSuccess)
        {
            if (result.Error == AuthorizationErrors.NotFound)
                return NotFound(new { message = result.Error });
            if (result.Error == AuthorizationErrors.Forbidden)
                return Forbid();
            return BadRequest(new { message = result.Error });
        }

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = AppRoles.Master)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteCustomerCommand(new CustomerId(id));
        var result = await _mediator.Send(command);
        if (!result.IsSuccess)
            return BadRequest(new { message = result.Error });

        return NoContent();
    }

    [HttpGet]
    [Authorize(Roles = AppRoles.Master)]
    public async Task<IActionResult> GetAll()
    {
        var customers = await _mediator.Send(new GetAllCustomersQuery());
        if (!customers.IsSuccess)
        {
            if (customers.Error == AuthorizationErrors.Forbidden)
                return Forbid();
            return BadRequest(new { message = customers.Error });
        }

        return Ok(customers.Value);
    }
}
