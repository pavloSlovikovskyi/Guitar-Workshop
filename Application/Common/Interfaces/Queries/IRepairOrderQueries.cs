using Domain.RepairOrders;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Interfaces.Queries
{
    public interface IRepairOrderQueries
    {
        Task<RepairOrder?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<RepairOrder>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<RepairOrder>> GetByInstrumentIdAsync(Guid instrumentId, CancellationToken cancellationToken = default);
    }
}
