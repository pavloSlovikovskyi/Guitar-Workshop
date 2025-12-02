using Application.Common;
using Domain.Customers;
using MediatR;

namespace Application.Customers.Commands
{
    public record UpdateCustomerCommand(
        CustomerId Id,
        string FirstName,
        string LastName,
        string PhoneNumber,
        string Email
    ) : IRequest<Result>;
}
