using Domain.Instruments;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Interfaces.Queries
{
    public interface IInstrumentQueries
    {
        Task<Instrument?> GetByIdAsync(InstrumentId id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Instrument>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}
