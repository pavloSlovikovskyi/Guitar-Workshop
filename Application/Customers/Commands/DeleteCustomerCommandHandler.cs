using Application.Common;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;
using Domain.Customers;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Customers.Commands
{
    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, Result>
    {
        private readonly ICustomerRepository _repository;
        private readonly ICurrentUserService _currentUser;

        public DeleteCustomerCommandHandler(
            ICustomerRepository repository,
            ICurrentUserService currentUser)
        {
            _repository = repository;
            _currentUser = currentUser;
        }

        public async Task<Result> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            var guard = AccessGuard.EnsureMaster(_currentUser);
            if (!guard.IsSuccess)
                return guard;

            var customer = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (customer == null)
                return Result.Failure("Customer not found");

            await _repository.DeleteAsync(customer, cancellationToken);

            return Result.Success();
        }
    }
}
