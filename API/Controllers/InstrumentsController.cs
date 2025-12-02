using API.Dtos;
using Application.Common;
using Application.Instruments.Commands;
using Application.Instruments.Queries;
using Domain.Customers;
using Domain.Instruments;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/instruments")]
    public class InstrumentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public InstrumentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateInstrument([FromBody] CreateInstrumentRequest request, CancellationToken cancellationToken)
        {
            var command = new CreateInstrumentCommand(
                request.Model,
                request.SerialNumber,
                request.RecieveDate,
                request.Status,
                request.CustomerId.HasValue ? new CustomerId(request.CustomerId.Value) : null
            );
            var result = await _mediator.Send(command, cancellationToken);
            if (!result.IsSuccess)
                return BadRequest(new { message = result.Error });

            return CreatedAtAction(nameof(GetInstrumentById), new { id = result.Value.Value }, new { id = result.Value.Value });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllInstruments()
        {
            var query = new GetAllInstrumentsQuery();
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
                return BadRequest(new { message = result.Error });

            var response = result.Value?.Select(x => new InstrumentResponse(
                x.Id.Value,
                x.Model,
                x.SerialNumber,
                x.RecieveDate,
                x.Status,
                x.CustomerId != null && x.CustomerId != CustomerId.Empty() ? x.CustomerId.Value : (Guid?)null,
                x.CreatedAt,
                x.UpdatedAt
            ));

            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetInstrumentById(Guid id)
        {
            var query = new GetInstrumentByIdQuery(new InstrumentId(id));
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
                return NotFound(new { message = result.Error });

            var instrument = result.Value;
            return Ok(new InstrumentResponse(
                instrument.Id.Value,
                instrument.Model,
                instrument.SerialNumber,
                instrument.RecieveDate,
                instrument.Status,
                instrument.CustomerId != null && instrument.CustomerId != CustomerId.Empty() ? instrument.CustomerId.Value : (Guid?)null,
                instrument.CreatedAt,
                instrument.UpdatedAt
            ));
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateInstrument(Guid id, [FromBody] UpdateInstrumentRequest request)
        {
            var command = new UpdateInstrumentCommand(
                new InstrumentId(id),
                request.Model,
                request.SerialNumber,
                request.RecieveDate
            );

            var result = await _mediator.Send(command);
            if (!result.IsSuccess)
                return BadRequest(new { message = result.Error });

            return NoContent();
        }

        [HttpPatch("{id:guid}/status")]
        public async Task<IActionResult> UpdateInstrumentStatus(Guid id, [FromBody] UpdateInstrumentStatusRequest request)
        {
            var command = new UpdateInstrumentStatusCommand(new InstrumentId(id), request.Status);
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(new { message = result.Error });

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteInstrument(Guid id)
        {
            var command = new DeleteInstrumentCommand(new InstrumentId(id));
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(new { message = result.Error });

            return NoContent();
        }
    }
}
