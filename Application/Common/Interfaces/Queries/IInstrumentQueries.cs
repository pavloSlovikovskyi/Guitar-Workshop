using Domain.Instruments;

namespace Application.Common.Interfaces.Queries;

public interface IInstrumentQueries
{
    Task<Instrument?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Instrument>> GetAllAsync(CancellationToken cancellationToken = default);
}
