using Application.Common.Interfaces.Queries;
using Domain.ServiceTypes;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Queries
{
    public class ServiceTypeQueries : IServiceTypeQueries
    {
        private readonly ApplicationDbContext _context;

        public ServiceTypeQueries(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceType?> GetByIdAsync(ServiceTypeId id, CancellationToken cancellationToken = default)
        {
            return await _context.ServiceTypes
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<ServiceType>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.ServiceTypes
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
    }
}
