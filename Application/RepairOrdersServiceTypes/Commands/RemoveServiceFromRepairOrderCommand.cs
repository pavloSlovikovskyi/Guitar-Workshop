using Application.Common;
using MediatR;
using System;

namespace Application.RepairOrdersServiceTypes.Commands
{
    public record RemoveServiceFromRepairOrderCommand(Guid OrderId, Guid ServiceId) : IRequest<Result>;
}
