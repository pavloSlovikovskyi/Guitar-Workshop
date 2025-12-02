using Domain.ServiceTypes;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Interfaces.Queries
{
    public interface IServiceTypeQueries
    {
        Task<ServiceType?> GetByIdAsync(ServiceTypeId id, CancellationToken cancellationToken = default);
        Task<IEnumerable<ServiceType>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}
