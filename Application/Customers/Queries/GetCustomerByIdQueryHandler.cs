using Application.Common;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Queries;
using Domain.Customers;
using MediatR;

namespace Application.Customers.Queries;

public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, Result<Customer>>
{
    private readonly ICustomerQueries _queries;
    private readonly ICurrentUserService _currentUser;

    public GetCustomerByIdQueryHandler(ICustomerQueries queries, ICurrentUserService currentUser)
    {
        _queries = queries;
        _currentUser = currentUser;
    }

    public async Task<Result<Customer>> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        var customer = await _queries.GetByIdAsync(request.Id, cancellationToken);
        if (customer is null)
            return Result<Customer>.Failure(AuthorizationErrors.NotFound);

        if (!_currentUser.IsMaster)
        {
            var (guard, customerId) = await AccessGuard.RequireCustomerIdAsync(_currentUser, cancellationToken);
            if (!guard.IsSuccess)
                return Result<Customer>.Failure(guard.Error!);

            var ownership = AccessGuard.EnsureOwnCustomer(_currentUser, customer.Id, customerId);
            if (!ownership.IsSuccess)
                return Result<Customer>.Failure(ownership.Error!);
        }

        return Result<Customer>.Success(customer);
    }
}
