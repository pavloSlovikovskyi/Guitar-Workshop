using Domain.RepairOrders;
using Domain.Instruments;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Interfaces.Queries
{
    public interface IRepairOrderQueries
    {
        Task<RepairOrder?> GetByIdAsync(RepairOrderId id, CancellationToken cancellationToken = default);
        Task<IEnumerable<RepairOrder>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<RepairOrder>> GetByInstrumentIdAsync(InstrumentId instrumentId, CancellationToken cancellationToken = default);
    }
}
