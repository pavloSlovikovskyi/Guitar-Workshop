using Application.Common.Interfaces.Repositories;
using Domain.InstrumentPassports;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class InstrumentPassportRepository : IInstrumentPassportRepository
    {
        private readonly ApplicationDbContext _context;

        public InstrumentPassportRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<InstrumentPassport?> GetByIdAsync(InstrumentPassportId id, CancellationToken cancellationToken = default)
        {
            return await _context.InstrumentPassports
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<InstrumentPassport>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.InstrumentPassports
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(InstrumentPassport passport, CancellationToken cancellationToken = default)
        {
            await _context.InstrumentPassports.AddAsync(passport, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(InstrumentPassport passport, CancellationToken cancellationToken = default)
        {
            _context.InstrumentPassports.Update(passport);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> ExistsAsync(InstrumentPassportId id, CancellationToken cancellationToken = default)
        {
            return await _context.InstrumentPassports.AnyAsync(p => p.Id == id, cancellationToken);
        }

        public async Task DeleteAsync(InstrumentPassport passport, CancellationToken cancellationToken = default)
        {
            _context.InstrumentPassports.Remove(passport);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
