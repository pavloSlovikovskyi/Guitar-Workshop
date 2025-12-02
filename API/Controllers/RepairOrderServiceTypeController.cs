using API.Dtos;
using Application.RepairOrders.Commands;
using Application.RepairOrdersServiceTypes.Commands;
using Application.RepairOrdersServiceTypes.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Domain.RepairOrders;
using Domain.ServiceTypes;

namespace API.Controllers
{
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
            var command = new AddServiceToRepairOrderCommand(
                new RepairOrderId(orderId),
                new ServiceTypeId(request.ServiceId)
            );
            var result = await _mediator.Send(command);
            if (!result.IsSuccess)
                return BadRequest(new { message = result.Error });
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveServiceFromOrder(Guid orderId, [FromBody] RemoveServiceFromOrderRequest request)
        {
            var command = new RemoveServiceFromRepairOrderCommand(
                new RepairOrderId(orderId),
                new ServiceTypeId(request.ServiceId)
            );
            var result = await _mediator.Send(command);
            if (!result.IsSuccess)
                return BadRequest(new { message = result.Error });
            return NoContent();
        }
    }
}
