using Application.Common;
using Domain.Enums;
using Domain.Instruments;
using Domain.RepairOrders;
using MediatR;

namespace Application.RepairOrders.Commands
{
    public record CreateRepairOrderCommand(
        InstrumentId InstrumentId,
        DateTime OrderDate,
        RepairOrderStatus Status,
        string Notes
    ) : IRequest<Result<RepairOrderId>>;
}
