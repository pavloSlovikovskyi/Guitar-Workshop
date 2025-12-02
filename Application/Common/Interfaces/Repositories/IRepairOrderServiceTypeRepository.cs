using Domain.RepairOrders;
using Domain.RepairOrdersServiceTypes;
using Domain.ServiceTypes;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Interfaces.Repositories
{
    public interface IRepairOrderServiceTypeRepository
    {
        Task AddAsync(RepairOrderServiceType entity, CancellationToken cancellationToken = default);
        Task DeleteAsync(RepairOrderServiceType entity, CancellationToken cancellationToken = default);
        Task<IEnumerable<RepairOrderServiceType>> GetByOrderIdAsync(RepairOrderId orderId, CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(RepairOrderId orderId, ServiceTypeId serviceId, CancellationToken cancellationToken = default);
        Task<IEnumerable<RepairOrderServiceType>> GetByServiceIdAsync(ServiceTypeId serviceId, CancellationToken cancellationToken = default);
    }
}
