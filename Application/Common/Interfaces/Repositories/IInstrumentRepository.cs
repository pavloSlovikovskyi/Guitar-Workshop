using Domain.Instruments;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Interfaces.Repositories
{
    public interface IInstrumentRepository
    {
        Task<Instrument?> GetByIdAsync(InstrumentId id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Instrument>> GetAllAsync(CancellationToken cancellationToken = default);
        Task AddAsync(Instrument instrument, CancellationToken cancellationToken = default);
        Task UpdateAsync(Instrument instrument, CancellationToken cancellationToken = default);
        Task DeleteAsync(Instrument instrument, CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(InstrumentId id, CancellationToken cancellationToken = default);
    }
}
