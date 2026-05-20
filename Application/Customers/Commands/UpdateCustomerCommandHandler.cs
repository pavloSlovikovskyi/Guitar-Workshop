using Application.Common;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;
using MediatR;

namespace Application.Customers.Commands;

public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, Result>
{
    private readonly ICustomerRepository _repository;
    private readonly ICurrentUserService _currentUser;

    public UpdateCustomerCommandHandler(
        ICustomerRepository repository,
        ICurrentUserService currentUser)
    {
        _repository = repository;
        _currentUser = currentUser;
    }

    public async Task<Result> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (customer is null)
            return Result.Failure(AuthorizationErrors.NotFound);

        if (!_currentUser.IsMaster)
        {
            var (guard, customerId) = await AccessGuard.RequireCustomerIdAsync(_currentUser, cancellationToken);
            if (!guard.IsSuccess)
                return guard;

            var ownership = AccessGuard.EnsureOwnCustomer(_currentUser, customer.Id, customerId);
            if (!ownership.IsSuccess)
                return ownership;
        }

        customer.UpdateDetails(
            request.FirstName,
            request.LastName,
            request.PhoneNumber,
            request.Email);

        await _repository.UpdateAsync(customer, cancellationToken);

        return Result.Success();
    }
}
