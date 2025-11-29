using Application.Common.Interfaces.Queries;
using Domain.Instruments;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Queries;

public class InstrumentQueries : IInstrumentQueries
{
    private readonly ApplicationDbContext _context;

    public InstrumentQueries(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Instrument?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Instruments
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Instrument>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Instruments
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}
