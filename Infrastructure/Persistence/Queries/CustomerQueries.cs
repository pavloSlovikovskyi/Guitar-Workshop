using Application.Common.Interfaces.Queries;
using Domain.Customers;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Queries
{
    public class CustomerQueries : ICustomerQueries
    {
        private readonly ApplicationDbContext _context;

        public CustomerQueries(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Customer?> GetByIdAsync(CustomerId id, CancellationToken cancellationToken = default)
        {
            return await _context.Customers
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<Customer>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Customers
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
    }
}
