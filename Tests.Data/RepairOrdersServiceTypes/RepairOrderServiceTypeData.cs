using Domain.RepairOrdersServiceTypes;

namespace Tests.Data.RepairOrdersServiceTypes;

public static class RepairOrderServiceTypeData
{
    public static RepairOrderServiceType CreateLink(Guid orderId, Guid serviceId) =>
        new RepairOrderServiceType(orderId, serviceId);
}
