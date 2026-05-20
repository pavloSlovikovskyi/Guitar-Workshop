using Application.Common;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Queries;
using Domain.Customers;
using MediatR;

namespace Application.Customers.Queries;

public class GetCurrentCustomerQueryHandler : IRequestHandler<GetCurrentCustomerQuery, Result<Customer>>
{
    private readonly ICurrentUserService _currentUser;
    private readonly ICustomerQueries _queries;

    public GetCurrentCustomerQueryHandler(ICurrentUserService currentUser, ICustomerQueries queries)
    {
        _currentUser = currentUser;
        _queries = queries;
    }

    public async Task<Result<Customer>> Handle(GetCurrentCustomerQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.GetUserId();
        if (userId is null)
            return Result<Customer>.Failure(AuthorizationErrors.NotFound);

        var customer = await _queries.GetByIdentityIdAsync(userId, cancellationToken);
        if (customer is null)
            return Result<Customer>.Failure(AuthorizationErrors.NotFound);

        return Result<Customer>.Success(customer);
    }
}
