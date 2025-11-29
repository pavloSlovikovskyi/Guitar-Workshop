using Application.Common.Interfaces.Queries;
using Domain.RepairOrders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Queries;

public class RepairOrderQueries : IRepairOrderQueries
{
    private readonly ApplicationDbContext _context;

    public RepairOrderQueries(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<RepairOrder?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.RepairOrders
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<RepairOrder>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.RepairOrders
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<RepairOrder>> GetByInstrumentIdAsync(Guid instrumentId, CancellationToken cancellationToken = default)
    {
        return await _context.RepairOrders
            .AsNoTracking()
            .Where(r => r.InstrumentId == instrumentId)
            .ToListAsync(cancellationToken);
    }
}
