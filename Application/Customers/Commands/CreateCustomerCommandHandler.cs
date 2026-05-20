using Application.Common;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;
using Domain.Customers;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Customers.Commands
{
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Result<CustomerId>>
    {
        private readonly ICustomerRepository _repository;
        private readonly ICurrentUserService _currentUser;

        public CreateCustomerCommandHandler(
            ICustomerRepository repository,
            ICurrentUserService currentUser)
        {
            _repository = repository;
            _currentUser = currentUser;
        }

        public async Task<Result<CustomerId>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var guard = AccessGuard.EnsureMaster(_currentUser);
            if (!guard.IsSuccess)
                return Result<CustomerId>.Failure(guard.Error!);

            var customer = Customer.New(
                CustomerId.New(),
                request.FirstName,
                request.LastName,
                request.PhoneNumber,
                request.Email
            );

            await _repository.AddAsync(customer, cancellationToken);

            return Result<CustomerId>.Success(customer.Id);
        }
    }
}
