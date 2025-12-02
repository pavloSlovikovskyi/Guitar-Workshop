using Domain.ServiceTypes;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Interfaces.Repositories
{
    public interface IServiceTypeRepository
    {
        Task<ServiceType?> GetByIdAsync(ServiceTypeId id, CancellationToken cancellationToken = default);
        Task<IEnumerable<ServiceType>> GetAllAsync(CancellationToken cancellationToken = default);
        Task AddAsync(ServiceType serviceType, CancellationToken cancellationToken = default);
        Task UpdateAsync(ServiceType serviceType, CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(ServiceTypeId id, CancellationToken cancellationToken = default);
        Task DeleteAsync(ServiceType serviceType, CancellationToken cancellationToken = default);
    }
}
