using API.Dtos;
using Application.Instruments.Commands;
using Application.Instruments.Queries;
using MediatR;
using Application.Common;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

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
            request.Model, request.SerialNumber, request.RecieveDate, request.Status, request.CustomerId
        );
        var result = await _mediator.Send(command, cancellationToken);
        if (!result.IsSuccess)
            return BadRequest(new { message = result.Error });
        return CreatedAtAction(nameof(GetInstrumentById), new { id = result.Value }, new { id = result.Value });
    }

    [HttpGet]
    public async Task<IActionResult> GetAllInstruments()
    {
        var query = new GetAllInstrumentsQuery();
        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
            return BadRequest(new { message = result.Error });
        var response = result.Value?.Select(x => new InstrumentResponse(
            x.Id, x.Model, x.SerialNumber, x.RecieveDate, x.Status, x.CustomerId, x.CreatedAt, x.UpdatedAt
        ));
        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetInstrumentById(Guid id)
    {
        var query = new GetInstrumentByIdQuery(id);
        var result = await _mediator.Send(query);
        if (!result.IsSuccess)
            return NotFound(new { message = result.Error });
        var instrument = result.Value;
        return Ok(new InstrumentResponse(
            instrument.Id, instrument.Model, instrument.SerialNumber, instrument.RecieveDate,
            instrument.Status, instrument.CustomerId, instrument.CreatedAt, instrument.UpdatedAt
        ));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateInstrument(Guid id, [FromBody] UpdateInstrumentRequest request)
    {
        var command = new UpdateInstrumentCommand(
            id, request.Model, request.SerialNumber, request.RecieveDate
        );
        var result = await _mediator.Send(command);
        if (!result.IsSuccess)
            return BadRequest(new { message = result.Error });
        return NoContent();
    }

    [HttpPatch("{id:guid}/status")]
    public async Task<IActionResult> UpdateInstrumentStatus(Guid id, [FromBody] UpdateInstrumentStatusRequest request)
    {
        var command = new UpdateInstrumentStatusCommand(id, request.Status);
        var result = await _mediator.Send(command);
        if (!result.IsSuccess)
            return BadRequest(new { message = result.Error });
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteInstrument(Guid id)
    {
        var command = new DeleteInstrumentCommand(id);
        var result = await _mediator.Send(command);
        if (!result.IsSuccess)
            return BadRequest(new { message = result.Error });
        return NoContent();
    }
}
