using API.Dtos;
using Application.RepairOrders.Commands;
using Application.RepairOrders.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/orders")]
public class RepairOrderController : ControllerBase
{
    private readonly IMediator _mediator;

    public RepairOrderController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRepairOrderRequest request)
    {
        var result = await _mediator.Send(new CreateRepairOrderCommand(request.InstrumentId, request.OrderDate, request.Status, request.Notes));
        if (!result.IsSuccess)
            return BadRequest(new { message = result.Error });
        return CreatedAtAction(nameof(GetById), new { id = result.Value }, new { id = result.Value });
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllRepairOrdersQuery());
        if (!result.IsSuccess)
            return BadRequest(new { message = result.Error });
        return Ok(result.Value);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetRepairOrderByIdQuery(id));
        if (!result.IsSuccess)
            return NotFound(new { message = result.Error });
        return Ok(result.Value);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRepairOrderRequest request)
    {
        var result = await _mediator.Send(new UpdateRepairOrderCommand(id, request.InstrumentId, request.OrderDate, request.Status, request.Notes));
        if (!result.IsSuccess)
            return BadRequest(new { message = result.Error });
        return NoContent();
    }

    [HttpPatch("{id:guid}/status")]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateRepairOrderStatusRequest request)
    {
        var result = await _mediator.Send(new UpdateRepairOrderStatusCommand(id, request.Status));
        if (!result.IsSuccess)
            return BadRequest(new { message = result.Error });
        return NoContent();
    }
}
