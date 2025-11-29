using Application.Common;
using Domain.RepairOrders;
using MediatR;
using System;

namespace Application.RepairOrders.Queries;

public record GetRepairOrderByIdQuery(Guid Id) : IRequest<Result<RepairOrder>>;
