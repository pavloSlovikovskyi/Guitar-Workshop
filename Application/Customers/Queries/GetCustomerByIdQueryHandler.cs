using Application.Common;
using Application.Common.Interfaces.Queries;
using Domain.Customers;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Customers.Queries
{
    public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, Result<Customer>>
    {
        private readonly ICustomerQueries _queries;

        public GetCustomerByIdQueryHandler(ICustomerQueries queries)
        {
            _queries = queries;
        }

        public async Task<Result<Customer>> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            var customer = await _queries.GetByIdAsync(request.Id, cancellationToken);
            if (customer == null)
                return Result<Customer>.Failure("Customer not found");
            return Result<Customer>.Success(customer);
        }
    }
}
