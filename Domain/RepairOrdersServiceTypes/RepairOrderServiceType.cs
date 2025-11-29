using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.RepairOrdersServiceTypes
{
    public class RepairOrderServiceType
    {
        public Guid OrderId { get; }
        public Guid ServiceId { get; }

        public RepairOrderServiceType(Guid orderId, Guid serviceId)
        {
            OrderId = orderId;
            ServiceId = serviceId;
        }
    }

}
