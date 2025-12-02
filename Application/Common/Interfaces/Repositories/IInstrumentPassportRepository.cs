using Domain.InstrumentPassports;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Interfaces.Repositories
{
    public interface IInstrumentPassportRepository
    {
        Task<InstrumentPassport?> GetByIdAsync(InstrumentPassportId id, CancellationToken cancellationToken = default);
        Task<IEnumerable<InstrumentPassport>> GetAllAsync(CancellationToken cancellationToken = default);
        Task AddAsync(InstrumentPassport passport, CancellationToken cancellationToken = default);
        Task UpdateAsync(InstrumentPassport passport, CancellationToken cancellationToken = default);
        Task DeleteAsync(InstrumentPassport passport, CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(InstrumentPassportId id, CancellationToken cancellationToken = default);
    }
}
