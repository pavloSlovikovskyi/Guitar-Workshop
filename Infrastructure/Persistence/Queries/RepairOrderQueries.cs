using Application.Common.Interfaces.Queries;
using Domain.Instruments;
using Domain.RepairOrders;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Queries
{
    public class RepairOrderQueries : IRepairOrderQueries
    {
        private readonly ApplicationDbContext _context;

        public RepairOrderQueries(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<RepairOrder?> GetByIdAsync(RepairOrderId id, CancellationToken cancellationToken = default)
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

        public async Task<IEnumerable<RepairOrder>> GetByInstrumentIdAsync(InstrumentId instrumentId, CancellationToken cancellationToken = default)
        {
            return await _context.RepairOrders
                .AsNoTracking()
                .Where(r => r.InstrumentId == instrumentId)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<RepairOrder>> GetAllWithIncludesAsync(CancellationToken cancellationToken)
        {
            return await _context.RepairOrders
                .Include(o => o.Instrument)
                .Include(o => o.RepairOrderServiceTypes)
                    .ThenInclude(ros => ros.ServiceType)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync(cancellationToken);
        }
    }
}
