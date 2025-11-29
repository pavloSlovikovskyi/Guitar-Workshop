using Application.Common.Interfaces.Repositories;
using Domain.ServiceTypes;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class ServiceTypeRepository : IServiceTypeRepository
{
    private readonly ApplicationDbContext _context;

    public ServiceTypeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ServiceType?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.ServiceTypes
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<ServiceType>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.ServiceTypes
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(ServiceType serviceType, CancellationToken cancellationToken = default)
    {
        await _context.ServiceTypes.AddAsync(serviceType, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(ServiceType serviceType, CancellationToken cancellationToken = default)
    {
        _context.ServiceTypes.Update(serviceType);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.ServiceTypes.AnyAsync(s => s.Id == id, cancellationToken);
    }

    public async Task DeleteAsync(ServiceType serviceType, CancellationToken cancellationToken = default)
    {
        _context.ServiceTypes.Remove(serviceType);
        await _context.SaveChangesAsync(cancellationToken);
    }

}
