using Domain.RepairOrdersServiceTypes;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Interfaces.Queries
{
    public interface IRepairOrderServiceTypeQueries
    {
        Task<IEnumerable<RepairOrderServiceType>> GetByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default);
        Task<IEnumerable<RepairOrderServiceType>> GetByServiceIdAsync(Guid serviceId, CancellationToken cancellationToken = default);
    }
}
