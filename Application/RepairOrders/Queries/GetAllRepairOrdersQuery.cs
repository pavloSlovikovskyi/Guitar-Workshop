using Application.Common;
using Application.RepairOrders.Dtos;
using Domain.RepairOrders;
using MediatR;
using System.Collections.Generic;

namespace Application.RepairOrders.Queries
{
    public record GetAllRepairOrdersQuery() : IRequest<Result<List<RepairOrderResponse>>>;
}
