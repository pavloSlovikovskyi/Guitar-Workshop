using API.Dtos;
using Application.ServiceTypes.Commands;
using Application.ServiceTypes.Queries;
using Domain.ServiceTypes;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/services")]
    public class ServiceTypeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ServiceTypeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
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
        public async Task<IActionResult> GetAllServiceTypes()
        {
            var query = new GetAllServiceTypesQuery();
            var result = await _mediator.Send(query);

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
        public async Task<IActionResult> GetServiceTypeById(Guid id)
        {
            var query = new GetServiceTypeByIdQuery(new ServiceTypeId(id));
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
                return NotFound(new { message = result.Error });

            var s = result.Value;
            var response = new ServiceTypeResponse(
                s.Id.Value,
                s.Title,
                s.Description,
                s.Price
            );

            return Ok(response);
        }

        [HttpPut("{id:guid}")]
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
        public async Task<IActionResult> DeleteServiceType(Guid id)
        {
            var command = new DeleteServiceTypeCommand(new ServiceTypeId(id));
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(new { message = result.Error });

            return NoContent();
        }
    }
}
