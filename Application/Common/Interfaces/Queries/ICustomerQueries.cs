using Domain.Customers;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Interfaces.Queries
{
    public interface ICustomerQueries
    {
        Task<Customer?> GetByIdAsync(CustomerId id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Customer>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}
