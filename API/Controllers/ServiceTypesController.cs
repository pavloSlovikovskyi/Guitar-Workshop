using API.Dtos;
using Application.Common;
using Application.ServiceTypes.Commands;
using Application.ServiceTypes.Queries;
using Domain.ServiceTypes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/services")]
[Authorize]
public class ServiceTypeController : ControllerBase
{
    private readonly IMediator _mediator;

    public ServiceTypeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize(Roles = AppRoles.Master)]
    public async Task<IActionResult> CreateServiceType([FromBody] CreateServiceTypeRequest request)
    {
        var command = new CreateServiceTypeCommand(
            request.Title,
            request.Description,
            request.Price
        );

        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(new { message = result.Error });

        return CreatedAtAction(nameof(GetServiceTypeById), new { id = result.Value }, new { id = result.Value });
    }

    [HttpGet]
    [Authorize(Roles = $"{AppRoles.Master},{AppRoles.Customer}")]
    public async Task<IActionResult> GetAllServiceTypes()
    {
        var result = await _mediator.Send(new GetAllServiceTypesQuery());

        if (!result.IsSuccess)
            return BadRequest(new { message = result.Error });

        var response = result.Value?.Select(s => new ServiceTypeResponse(
            s.Id.Value,
            s.Title,
            s.Description,
            s.Price
        ));

        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    [Authorize(Roles = $"{AppRoles.Master},{AppRoles.Customer}")]
    public async Task<IActionResult> GetServiceTypeById(Guid id)
    {
        var result = await _mediator.Send(new GetServiceTypeByIdQuery(new ServiceTypeId(id)));

        if (!result.IsSuccess)
            return NotFound(new { message = result.Error });

        var s = result.Value!;
        return Ok(new ServiceTypeResponse(
            s.Id.Value,
            s.Title,
            s.Description,
            s.Price
        ));
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = AppRoles.Master)]
    public async Task<IActionResult> UpdateServiceType(Guid id, [FromBody] UpdateServiceTypeRequest request)
    {
        var command = new UpdateServiceTypeCommand(
            new ServiceTypeId(id),
            request.Title,
            request.Description,
            request.Price
        );

        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(new { message = result.Error });

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = AppRoles.Master)]
    public async Task<IActionResult> DeleteServiceType(Guid id)
    {
        var command = new DeleteServiceTypeCommand(new ServiceTypeId(id));
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(new { message = result.Error });

        return NoContent();
    }
}
