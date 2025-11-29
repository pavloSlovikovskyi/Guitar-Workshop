using Domain.RepairOrdersServiceTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces.Repositories
{
    public interface IRepairOrderServiceTypeRepository
    {
        Task AddAsync(RepairOrderServiceType entity, CancellationToken cancellationToken = default);
        Task DeleteAsync(RepairOrderServiceType entity, CancellationToken cancellationToken = default);
        Task<IEnumerable<RepairOrderServiceType>> GetByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(Guid orderId, Guid serviceId, CancellationToken cancellationToken = default);
        Task<IEnumerable<RepairOrderServiceType>> GetByServiceIdAsync(Guid serviceId, CancellationToken cancellationToken = default);

    }

}
