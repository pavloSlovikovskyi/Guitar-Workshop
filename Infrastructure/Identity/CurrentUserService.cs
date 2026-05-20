using Application.Common;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Queries;
using Domain.Customers;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Infrastructure.Identity;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ICustomerQueries _customerQueries;

    private CustomerId? _cachedCustomerId;
    private bool _customerIdResolved;

    public CurrentUserService(
        IHttpContextAccessor httpContextAccessor,
        ICustomerQueries customerQueries)
    {
        _httpContextAccessor = httpContextAccessor;
        _customerQueries = customerQueries;
    }

    public string? GetUserId() =>
        _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

    public bool IsAuthenticated =>
        _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated == true;

    public bool IsMaster => IsInRole(AppRoles.Master);

    public bool IsCustomer => IsInRole(AppRoles.Customer);

    public async Task<CustomerId?> GetCustomerIdAsync(CancellationToken cancellationToken = default)
    {
        if (_customerIdResolved)
            return _cachedCustomerId;

        _customerIdResolved = true;

        var userId = GetUserId();
        if (userId is null)
            return null;

        var customer = await _customerQueries.GetByIdentityIdAsync(userId, cancellationToken);
        _cachedCustomerId = customer?.Id;
        return _cachedCustomerId;
    }

    private bool IsInRole(string role) =>
        _httpContextAccessor.HttpContext?.User.IsInRole(role) == true;
}
