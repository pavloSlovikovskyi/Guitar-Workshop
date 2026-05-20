using Application.Common;
using Domain.Customers;
using MediatR;

namespace Application.Customers.Queries;

public record GetCurrentCustomerQuery : IRequest<Result<Customer>>;
