using System;

namespace API.Dtos
{
    public record CreateCustomerRequest(
        string FirstName,
        string LastName,
        string PhoneNumber,
        string Email
    );

    public record UpdateCustomerRequest(
        string FirstName,
        string LastName,
        string PhoneNumber,
        string Email
    );

    public record CustomerResponse(
        Guid Id,
        string FirstName,
        string LastName,
        string PhoneNumber,
        string Email,
        DateTime CreatedAt,
        DateTime? UpdatedAt
    )
    {
        public static CustomerResponse FromDomainModel(Domain.Customers.Customer model)
            => new(
                model.Id.Value,
                model.FirstName,
                model.LastName,
                model.PhoneNumber,
                model.Email,
                model.CreatedAt,
                model.UpdatedAt
            );
    }
}
