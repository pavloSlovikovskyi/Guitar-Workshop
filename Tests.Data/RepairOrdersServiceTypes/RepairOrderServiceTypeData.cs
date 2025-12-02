using Domain.RepairOrdersServiceTypes;
using Domain.RepairOrders;
using Domain.ServiceTypes;

namespace Tests.Data.RepairOrdersServiceTypes;

public static class RepairOrderServiceTypeData
{
    public static RepairOrderServiceType CreateLink(RepairOrderId orderId, ServiceTypeId serviceId) =>
        RepairOrderServiceType.New(orderId, serviceId);
}
