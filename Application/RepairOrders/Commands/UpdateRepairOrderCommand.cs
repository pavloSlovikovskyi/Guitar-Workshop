using Application.Common;
using Domain.Enums;
using Domain.RepairOrders;
using Domain.Instruments;
using MediatR;
using System;

namespace Application.RepairOrders.Commands
{
    public record UpdateRepairOrderCommand(
        RepairOrderId Id,
        InstrumentId InstrumentId,
        DateTime OrderDate,
        RepairOrderStatus Status,
        string Notes
    ) : IRequest<Result>;
}
