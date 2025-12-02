using Application.Common.Interfaces.Repositories;
using Domain.Instruments;
using Domain.RepairOrders;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class RepairOrderRepository : IRepairOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public RepairOrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<RepairOrder?> GetByIdAsync(RepairOrderId id, CancellationToken cancellationToken = default)
        {
            return await _context.RepairOrders
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

        public async Task AddAsync(RepairOrder repairOrder, CancellationToken cancellationToken = default)
        {
            await _context.RepairOrders.AddAsync(repairOrder, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(RepairOrder repairOrder, CancellationToken cancellationToken = default)
        {
            _context.RepairOrders.Update(repairOrder);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> ExistsAsync(RepairOrderId id, CancellationToken cancellationToken = default)
        {
            return await _context.RepairOrders.AnyAsync(r => r.Id == id, cancellationToken);
        }
    }
}
