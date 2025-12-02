using Application.Common;
using Domain.RepairOrders;
using Domain.ServiceTypes;
using MediatR;

namespace Application.RepairOrdersServiceTypes.Commands
{
    public record AddServiceToRepairOrderCommand(RepairOrderId OrderId, ServiceTypeId ServiceId) : IRequest<Result>;
}
