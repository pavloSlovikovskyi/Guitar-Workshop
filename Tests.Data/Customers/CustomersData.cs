using API.Dtos;
using Domain.Customers;

namespace Tests.Data.Customers;

public static class CustomerData
{
    public static Customer FirstTestCustomer() =>
        Customer.New(
            CustomerId.New(),
            "Іван",
            "Іванов",
            "+380501234567",
            "ivan.ivanov@example.com"
        );

    public static Customer SecondTestCustomer() =>
        Customer.New(
            CustomerId.New(),
            "Олена",
            "Петренко",
            "+380671234567",
            "olena.petrenko@example.com"
        );

    public static CreateCustomerRequest CreateValidRequest(
        string? firstName = null,
        string? lastName = null,
        string? phoneNumber = null,
        string? email = null) =>
        new CreateCustomerRequest(
            FirstName: firstName ?? "Петро",
            LastName: lastName ?? "Сидоренко",
            PhoneNumber: phoneNumber ?? "+380631234567",
            Email: email ?? $"petro.sydorenko{Guid.NewGuid().ToString()[..8]}@example.com"
        );
}
