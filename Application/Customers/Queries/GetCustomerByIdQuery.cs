using Application.Common;
using Domain.Customers;
using MediatR;

namespace Application.Customers.Queries
{
    public record GetCustomerByIdQuery(CustomerId Id) : IRequest<Result<Customer>>;
}
