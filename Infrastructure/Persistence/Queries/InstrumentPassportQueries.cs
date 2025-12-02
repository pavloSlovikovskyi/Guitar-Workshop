using Application.Common.Interfaces.Queries;
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

        public async Task<IEnumerable<InstrumentPassport>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.InstrumentPassports
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
    }
}
