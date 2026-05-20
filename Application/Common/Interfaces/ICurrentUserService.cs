using Domain.Customers;

namespace Application.Common.Interfaces;

public interface ICurrentUserService
{
    string? GetUserId();
    bool IsAuthenticated { get; }
    bool IsMaster { get; }
    bool IsCustomer { get; }
    Task<CustomerId?> GetCustomerIdAsync(CancellationToken cancellationToken = default);
}
