using API.Dtos;
using Application.Common;
using Application.Customers.Commands;
using Application.Customers.Queries;
using Domain.Customers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/customers")]
    public class CustomerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CustomerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
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

            return CreatedAtAction(nameof(GetById), new { id = result.Value.Value }, new { id = result.Value.Value });
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var query = new GetCustomerByIdQuery(new CustomerId(id));
            var result = await _mediator.Send(query);
            if (!result.IsSuccess)
                return NotFound(new { message = result.Error });

            var response = CustomerResponse.FromDomainModel(result.Value);
            return Ok(response);
        }

        [HttpPut("{id:guid}")]
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
                return BadRequest(new { message = result.Error });

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteCustomerCommand(new CustomerId(id));
            var result = await _mediator.Send(command);
            if (!result.IsSuccess)
                return BadRequest(new { message = result.Error });

            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var customers = await _mediator.Send(new GetAllCustomersQuery());
            if (!customers.IsSuccess)
                return BadRequest(new { message = customers.Error });

            return Ok(customers.Value);
        }

    }
}
