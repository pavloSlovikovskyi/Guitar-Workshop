using API.Dtos;
using Application.Common;
using Application.RepairOrdersServiceTypes.Commands;
using Domain.RepairOrders;
using Domain.ServiceTypes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/orders/{orderId:guid}/services")]
[Authorize(Roles = AppRoles.Master)]
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
        var command = new AddServiceToRepairOrderCommand(
            new RepairOrderId(orderId),
            new ServiceTypeId(request.ServiceId)
        );
        var result = await _mediator.Send(command);
        if (!result.IsSuccess)
            return BadRequest(new { message = result.Error });
        return NoContent();
    }

    [HttpDelete("{serviceId:guid}")]
    public async Task<IActionResult> RemoveServiceFromOrder(Guid orderId, Guid serviceId)
    {
        var command = new RemoveServiceFromRepairOrderCommand(
            new RepairOrderId(orderId),
            new ServiceTypeId(serviceId)
        );

        var result = await _mediator.Send(command);
        if (!result.IsSuccess)
            return BadRequest(new { message = result.Error });

        return NoContent();
    }
}
