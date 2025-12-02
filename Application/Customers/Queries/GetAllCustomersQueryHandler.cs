using Application.Common;
using Application.Common.Interfaces.Queries;
using Domain.Customers;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Customers.Queries
{
    public class GetAllCustomersQueryHandler : IRequestHandler<GetAllCustomersQuery, Result<List<Customer>>>
    {
        private readonly ICustomerQueries _queries;

        public GetAllCustomersQueryHandler(ICustomerQueries queries)
        {
            _queries = queries;
        }

        public async Task<Result<List<Customer>>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
        {
            var customers = await _queries.GetAllAsync(cancellationToken);
            return Result<List<Customer>>.Success(customers.ToList());
        }
    }
}
