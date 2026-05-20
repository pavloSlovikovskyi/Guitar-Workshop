using Application.Common;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Queries;
using Domain.Customers;
using MediatR;

namespace Application.Customers.Queries;

public class GetAllCustomersQueryHandler : IRequestHandler<GetAllCustomersQuery, Result<List<Customer>>>
{
    private readonly ICustomerQueries _queries;
    private readonly ICurrentUserService _currentUser;

    public GetAllCustomersQueryHandler(ICustomerQueries queries, ICurrentUserService currentUser)
    {
        _queries = queries;
        _currentUser = currentUser;
    }

    public async Task<Result<List<Customer>>> Handle(
        GetAllCustomersQuery request,
        CancellationToken cancellationToken)
    {
        var guard = AccessGuard.EnsureMaster(_currentUser);
        if (!guard.IsSuccess)
            return Result<List<Customer>>.Failure(guard.Error!);

        var customers = await _queries.GetAllAsync(cancellationToken);
        return Result<List<Customer>>.Success(customers.ToList());
    }
}
