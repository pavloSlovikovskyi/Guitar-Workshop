using Application.Common;
using Domain.RepairOrdersServiceTypes;
using MediatR;
using System;
using System.Collections.Generic;

namespace Application.RepairOrdersServiceTypes.Queries
{
    public record GetServicesByOrderIdQuery(Guid OrderId) : IRequest<Result<List<RepairOrderServiceType>>>;
    public record GetOrdersByServiceIdQuery(Guid ServiceId) : IRequest<Result<List<RepairOrderServiceType>>>;
}
