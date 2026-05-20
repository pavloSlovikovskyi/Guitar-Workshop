using Application.Common.Interfaces.Queries;
using Domain.Customers;
using Domain.InstrumentPassports;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Queries
{
    public class InstrumentPassportQueries : IInstrumentPassportQueries
    {
        private readonly ApplicationDbContext _context;

        public InstrumentPassportQueries(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<InstrumentPassport?> GetByIdAsync(InstrumentPassportId id, CancellationToken cancellationToken = default)
        {
            return await _context.InstrumentPassports
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<InstrumentPassport?> GetByIdWithInstrumentAsync(
            InstrumentPassportId id,
            CancellationToken cancellationToken = default)
        {
            return await _context.InstrumentPassports
                .AsNoTracking()
                .Include(p => p.Instrument)
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<InstrumentPassport>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.InstrumentPassports
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<InstrumentPassport>> GetAllByCustomerIdAsync(
            CustomerId customerId,
            CancellationToken cancellationToken = default)
        {
            return await _context.InstrumentPassports
                .AsNoTracking()
                .Where(p => _context.Instruments.Any(i => i.Id == p.InstrumentId && i.CustomerId == customerId))
                .ToListAsync(cancellationToken);
        }
    }
}
