using Application.Common.Interfaces.Repositories;
using Domain.Instruments;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class InstrumentRepository : IInstrumentRepository
    {
        private readonly ApplicationDbContext _context;

        public InstrumentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Instrument?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Instruments
                .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<Instrument>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Instruments
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(Instrument instrument, CancellationToken cancellationToken = default)
        {
            await _context.Instruments.AddAsync(instrument, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Instrument instrument, CancellationToken cancellationToken = default)
        {
            _context.Instruments.Update(instrument);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Instrument instrument, CancellationToken cancellationToken = default)
        {
            _context.Instruments.Remove(instrument);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Instruments.AnyAsync(i => i.Id == id, cancellationToken);
        }
    }
}
