using Domain.RepairOrders;
using Domain.ServiceTypes;

namespace Domain.RepairOrdersServiceTypes
{
    public class RepairOrderServiceType
    {
        public RepairOrderId OrderId { get; }
        public RepairOrder Order { get; private set; }

        public ServiceTypeId ServiceId { get; }
        public ServiceType ServiceType { get; private set; }

        private RepairOrderServiceType(RepairOrderId orderId, ServiceTypeId serviceId)
        {
            OrderId = orderId;
            ServiceId = serviceId;
        }

        public static RepairOrderServiceType New(RepairOrderId orderId, ServiceTypeId serviceId)
            => new(orderId, serviceId);
    }
}
