using Application.Common.Interfaces.Repositories;
using Domain.Customers;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _context;

        public CustomerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Customer?> GetByIdAsync(CustomerId id, CancellationToken cancellationToken = default)
        {
            return await _context.Customers
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<Customer>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Customers
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(Customer customer, CancellationToken cancellationToken = default)
        {
            await _context.Customers.AddAsync(customer, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Customer customer, CancellationToken cancellationToken = default)
        {
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> ExistsAsync(CustomerId id, CancellationToken cancellationToken = default)
        {
            return await _context.Customers.AnyAsync(c => c.Id == id, cancellationToken);
        }

        public async Task DeleteAsync(Customer customer, CancellationToken cancellationToken = default)
        {
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
