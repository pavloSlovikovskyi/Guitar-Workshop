using Application.Common;
using Domain.Enums;
using Domain.RepairOrders;
using MediatR;

namespace Application.RepairOrders.Commands
{
    public record UpdateRepairOrderStatusCommand(RepairOrderId Id, RepairOrderStatus Status) : IRequest<Result>;
}
