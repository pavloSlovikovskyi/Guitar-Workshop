using API.Dtos;
using Application.Common;
using Application.RepairOrders.Commands;
using Application.RepairOrders.Queries;
using Domain.Instruments;
using Domain.RepairOrders;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Api.Controllers
{
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
            var command = new CreateRepairOrderCommand(
                new InstrumentId(request.InstrumentId),
                request.OrderDate,
                request.Status,
                request.Notes
            );

            var result = await _mediator.Send(command);
            if (!result.IsSuccess)
                return BadRequest(new { message = result.Error });

            return CreatedAtAction(nameof(GetById), new { id = result.Value.Value }, new { id = result.Value.Value });
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
            var result = await _mediator.Send(new GetRepairOrderByIdQuery(new RepairOrderId(id)));
            if (!result.IsSuccess)
                return NotFound(new { message = result.Error });

            return Ok(result.Value);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRepairOrderRequest request)
        {
            var command = new UpdateRepairOrderCommand(
                new RepairOrderId(id),
                new InstrumentId(request.InstrumentId),
                request.OrderDate,
                request.Status,
                request.Notes
            );

            var result = await _mediator.Send(command);
            if (!result.IsSuccess)
                return BadRequest(new { message = result.Error });

            return NoContent();
        }

        [HttpPatch("{id:guid}/status")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateRepairOrderStatusRequest request)
        {
            var command = new UpdateRepairOrderStatusCommand(new RepairOrderId(id), request.Status);
            var result = await _mediator.Send(command);
            if (!result.IsSuccess)
                return BadRequest(new { message = result.Error });

            return NoContent();
        }
    }
}

