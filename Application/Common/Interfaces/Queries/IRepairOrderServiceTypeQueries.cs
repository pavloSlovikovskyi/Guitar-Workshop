using Domain.RepairOrders;
using Domain.RepairOrdersServiceTypes;
using Domain.ServiceTypes;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Interfaces.Queries
{
    public interface IRepairOrderServiceTypeQueries
    {
        Task<IEnumerable<RepairOrderServiceType>> GetByOrderIdAsync(RepairOrderId orderId, CancellationToken cancellationToken = default);
        Task<IEnumerable<RepairOrderServiceType>> GetByServiceIdAsync(ServiceTypeId serviceId, CancellationToken cancellationToken = default);
    }
}
