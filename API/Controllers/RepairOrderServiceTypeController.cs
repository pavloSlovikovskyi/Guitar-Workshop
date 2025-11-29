using API.Dtos;
using Application.RepairOrdersServiceTypes.Queries;
using Application.RepairOrdersServiceTypes.Commands;
using Application.RepairOrders.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace API.Controllers;

[ApiController]
[Route("api/orders/{orderId:guid}/services")]
public class RepairOrderServiceTypeController : ControllerBase
{
    private readonly IMediator _mediator;

    public RepairOrderServiceTypeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> AddServiceToOrder(Guid orderId, [FromBody] AddServiceToOrderRequest request)
    {
        var command = new AddServiceToRepairOrderCommand(orderId, request.ServiceId);
        var result = await _mediator.Send(command);
        if (!result.IsSuccess)
            return BadRequest(new { message = result.Error });
        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> RemoveServiceFromOrder(Guid orderId, [FromBody] RemoveServiceFromOrderRequest request)
    {
        var command = new RemoveServiceFromRepairOrderCommand(orderId, request.ServiceId);
        var result = await _mediator.Send(command);
        if (!result.IsSuccess)
            return BadRequest(new { message = result.Error });
        return NoContent();
    }
}
