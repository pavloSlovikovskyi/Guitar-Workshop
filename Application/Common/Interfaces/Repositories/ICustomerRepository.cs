using Domain.Customers;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Interfaces.Repositories
{
    public interface ICustomerRepository
    {
        Task<Customer?> GetByIdAsync(CustomerId id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Customer>> GetAllAsync(CancellationToken cancellationToken = default);
        Task AddAsync(Customer customer, CancellationToken cancellationToken = default);
        Task UpdateAsync(Customer customer, CancellationToken cancellationToken = default);
        Task DeleteAsync(Customer customer, CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(CustomerId id, CancellationToken cancellationToken = default);
    }
}
