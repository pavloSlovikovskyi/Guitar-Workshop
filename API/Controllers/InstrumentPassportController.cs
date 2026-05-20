using API.Dtos;
using Application.Common;
using Application.InstrumentPassports.Commands;
using Application.InstrumentPassports.Queries;
using Domain.InstrumentPassports;
using Domain.Instruments;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/instrument-passports")]
[Authorize]
public class InstrumentPassportController : ControllerBase
{
    private readonly IMediator _mediator;

    public InstrumentPassportController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize(Roles = AppRoles.Master)]
    public async Task<IActionResult> Create([FromBody] CreateInstrumentPassportRequest request)
    {
        var command = new CreateInstrumentPassportCommand(
            new InstrumentId(request.InstrumentId),
            request.IssueDate,
            request.Details
        );

        var result = await _mediator.Send(command);
        if (!result.IsSuccess)
            return BadRequest(new { message = result.Error });

        return CreatedAtAction(nameof(GetById), new { id = result.Value!.Value }, new { id = result.Value.Value });
    }

    [HttpGet("{id:guid}")]
    [Authorize(Roles = $"{AppRoles.Master},{AppRoles.Customer}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetInstrumentPassportByIdQuery(new InstrumentPassportId(id)));
        if (!result.IsSuccess)
            return NotFound(new { message = result.Error });

        return Ok(InstrumentPassportResponse.FromDomainModel(result.Value!));
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = AppRoles.Master)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateInstrumentPassportRequest request)
    {
        var command = new UpdateInstrumentPassportCommand(
            new InstrumentPassportId(id),
            request.IssueDate,
            request.Details
        );

        var result = await _mediator.Send(command);
        if (!result.IsSuccess)
            return BadRequest(new { message = result.Error });

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = AppRoles.Master)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteInstrumentPassportCommand(new InstrumentPassportId(id));
        var result = await _mediator.Send(command);
        if (!result.IsSuccess)
            return BadRequest(new { message = result.Error });

        return NoContent();
    }

    [HttpGet]
    [Authorize(Roles = $"{AppRoles.Master},{AppRoles.Customer}")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllInstrumentPassportsQuery());

        if (!result.IsSuccess)
            return BadRequest(new { message = result.Error });

        var response = result.Value!.Select(InstrumentPassportResponse.FromDomainModel);
        return Ok(response);
    }
}
