using Application.Common;
using Domain.Enums;
using MediatR;
using System;

namespace Application.RepairOrders.Commands;

public record UpdateRepairOrderCommand(
    Guid Id,
    Guid InstrumentId,
    DateTime OrderDate,
    RepairOrderStatus Status,
    string Notes
) : IRequest<Result>;
