using Application.Common;
using Domain.RepairOrders;
using MediatR;

namespace Application.RepairOrders.Queries
{
    public record GetRepairOrderByIdQuery(RepairOrderId Id) : IRequest<Result<RepairOrder>>;
}
