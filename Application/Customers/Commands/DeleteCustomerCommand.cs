using Application.Common;
using Domain.Customers;
using MediatR;

namespace Application.Customers.Commands
{
    public record DeleteCustomerCommand(CustomerId Id) : IRequest<Result>;
}
