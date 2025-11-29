using Application.Common;
using MediatR;
using System;

namespace Application.RepairOrdersServiceTypes.Commands
{
    public record AddServiceToRepairOrderCommand(Guid OrderId, Guid ServiceId) : IRequest<Result>;
}
