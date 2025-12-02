using Application.Common.Interfaces.Queries;
using Domain.RepairOrders;
using Domain.RepairOrdersServiceTypes;
using Domain.ServiceTypes;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Queries
{
    public class RepairOrderServiceTypeQueries : IRepairOrderServiceTypeQueries
    {
        private readonly ApplicationDbContext _context;

        public RepairOrderServiceTypeQueries(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RepairOrderServiceType>> GetByOrderIdAsync(RepairOrderId orderId, CancellationToken cancellationToken = default)
        {
            return await _context.Set<RepairOrderServiceType>()
                .Where(x => x.OrderId == orderId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<RepairOrderServiceType>> GetByServiceIdAsync(ServiceTypeId serviceId, CancellationToken cancellationToken = default)
        {
            return await _context.Set<RepairOrderServiceType>()
                .Where(x => x.ServiceId == serviceId)
                .ToListAsync(cancellationToken);
        }
    }
}
