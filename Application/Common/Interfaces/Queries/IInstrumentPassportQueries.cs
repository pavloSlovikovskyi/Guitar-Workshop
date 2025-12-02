using Domain.InstrumentPassports;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Interfaces.Queries
{
    public interface IInstrumentPassportQueries
    {
        Task<InstrumentPassport?> GetByIdAsync(InstrumentPassportId id, CancellationToken cancellationToken = default);
        Task<IEnumerable<InstrumentPassport>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}
