using Application.Common;
using Domain.Customers;
using MediatR;
using System.Collections.Generic;

namespace Application.Customers.Queries
{
    public record GetAllCustomersQuery() : IRequest<Result<List<Customer>>>;
}
