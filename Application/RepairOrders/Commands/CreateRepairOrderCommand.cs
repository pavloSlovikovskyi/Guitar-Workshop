using Application.Common;
using Domain.Enums;
using MediatR;

namespace Application.RepairOrders.Commands;

public record CreateRepairOrderCommand(
    Guid InstrumentId,
    DateTime OrderDate,
    RepairOrderStatus Status,
    string Notes
) : IRequest<Result<Guid>>;
