using Application.Common;
using Application.Common.Interfaces.Repositories;
using Domain.Customers;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Customers.Commands
{
    public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, Result>
    {
        private readonly ICustomerRepository _repository;

        public UpdateCustomerCommandHandler(ICustomerRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (customer == null)
                return Result.Failure("Customer not found");

            customer.UpdateDetails(
                request.FirstName,
                request.LastName,
                request.PhoneNumber,
                request.Email
            );

            await _repository.UpdateAsync(customer, cancellationToken);

            return Result.Success();
        }
    }
}
