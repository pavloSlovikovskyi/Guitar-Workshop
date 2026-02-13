using Application.Common.Interfaces.Repositories;
using Domain.RepairOrders;
using Domain.RepairOrdersServiceTypes;
using Domain.ServiceTypes;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class RepairOrderServiceTypeRepository : IRepairOrderServiceTypeRepository
    {
        private readonly ApplicationDbContext _context;

        public RepairOrderServiceTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(RepairOrderServiceType entity, CancellationToken cancellationToken = default)
        {
            await _context.Set<RepairOrderServiceType>().AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(RepairOrderServiceType entity, CancellationToken cancellationToken = default)
        {
            _context.Set<RepairOrderServiceType>().Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<RepairOrderServiceType>> GetByOrderIdAsync(RepairOrderId orderId, CancellationToken cancellationToken = default)
        {
            return await _context.Set<RepairOrderServiceType>()
                .Where(x => x.OrderId == orderId)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> ExistsAsync(RepairOrderId orderId, ServiceTypeId serviceId, CancellationToken cancellationToken = default)
        {
            return await _context.Set<RepairOrderServiceType>()
                .AnyAsync(x => x.OrderId == orderId && x.ServiceId == serviceId, cancellationToken);
        }

        public async Task<IEnumerable<RepairOrderServiceType>> GetByServiceIdAsync(ServiceTypeId serviceId, CancellationToken cancellationToken = default)
        {
            return await _context.Set<RepairOrderServiceType>()
                .Where(x => x.ServiceId == serviceId)
                .ToListAsync(cancellationToken);
        }
        public async Task<RepairOrderServiceType?> GetByOrderAndServiceIdAsync(
            RepairOrderId orderId,
            ServiceTypeId serviceId,
            CancellationToken cancellationToken = default)
        {
            return await _context.Set<RepairOrderServiceType>()
                .FirstOrDefaultAsync(x => x.OrderId == orderId && x.ServiceId == serviceId, cancellationToken);
        }

    }
}
