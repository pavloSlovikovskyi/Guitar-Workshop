using Application.Common;
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

        public CreateCustomerCommandHandler(ICustomerRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<CustomerId>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
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
