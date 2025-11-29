using Application.Common;
using Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.RepairOrders.Commands
{
    public record UpdateRepairOrderStatusCommand(Guid Id, RepairOrderStatus Status) : IRequest<Result>;
}
