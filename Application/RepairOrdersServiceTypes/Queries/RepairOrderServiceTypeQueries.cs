using Application.Common;
using Domain.RepairOrders;
using Domain.RepairOrdersServiceTypes;
using Domain.ServiceTypes;
using MediatR;
using System.Collections.Generic;

namespace Application.RepairOrdersServiceTypes.Queries
{
    public record GetServicesByOrderIdQuery(RepairOrderId OrderId) : IRequest<Result<List<RepairOrderServiceType>>>;
    public record GetOrdersByServiceIdQuery(ServiceTypeId ServiceId) : IRequest<Result<List<RepairOrderServiceType>>>;
}
