using Application.Common.Interfaces;
using Domain.Customers;

namespace Application.Common;

public static class AccessGuard
{
    public static Result EnsureMaster(ICurrentUserService currentUser)
    {
        if (!currentUser.IsMaster)
            return Result.Failure(AuthorizationErrors.Forbidden);

        return Result.Success();
    }

    public static async Task<(Result Guard, CustomerId? CustomerId)> RequireCustomerIdAsync(
        ICurrentUserService currentUser,
        CancellationToken cancellationToken)
    {
        var customerId = await currentUser.GetCustomerIdAsync(cancellationToken);
        if (customerId is null)
            return (Result.Failure(AuthorizationErrors.NotFound), null);

        return (Result.Success(), customerId);
    }

    public static Result EnsureOwnCustomer(
        ICurrentUserService currentUser,
        CustomerId resourceCustomerId,
        CustomerId? currentCustomerId)
    {
        if (currentUser.IsMaster)
            return Result.Success();

        if (currentCustomerId is null || resourceCustomerId != currentCustomerId)
            return Result.Failure(AuthorizationErrors.NotFound);

        return Result.Success();
    }
}
