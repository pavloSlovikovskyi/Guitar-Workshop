using Domain.RepairOrders;
using Domain.Instruments;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Interfaces.Repositories
{
    public interface IRepairOrderRepository
    {
        Task<RepairOrder?> GetByIdAsync(RepairOrderId id, CancellationToken cancellationToken = default);
        Task<IEnumerable<RepairOrder>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<RepairOrder>> GetByInstrumentIdAsync(InstrumentId instrumentId, CancellationToken cancellationToken = default);
        Task AddAsync(RepairOrder repairOrder, CancellationToken cancellationToken = default);
        Task UpdateAsync(RepairOrder repairOrder, CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(RepairOrderId id, CancellationToken cancellationToken = default);
    }
}
