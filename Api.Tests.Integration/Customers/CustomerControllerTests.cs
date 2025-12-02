using API.Dtos;
using API.Dtos.Responses;
using Domain.Customers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using Tests.Common;
using Tests.Data.Customers;
using Xunit;

namespace Api.Tests.Integration;

public class CustomerControllerTests : BaseIntegrationTest, IAsyncLifetime
{
    private Customer _firstTestCustomer;
    private Customer _secondTestCustomer;

    private const string BaseRoute = "api/customers";

    public CustomerControllerTests(IntegrationTestWebFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task ShouldCreateCustomer()
    {
        var request = CustomerData.CreateValidRequest();

        var response = await Client.PostAsJsonAsync(BaseRoute, request);

        response.IsSuccessStatusCode.Should().BeTrue();

        var customer = await response.ToResponseModel<CustomerResponseDto>();
        var customerId = customer.Id;
        customerId.Should().NotBe(Guid.Empty);

        var customerDb = await Context.Customers.FirstAsync(x => x.Id == new CustomerId(customerId));
        customerDb.Email.Should().Be(request.Email);
    }

    [Fact]
    public async Task ShouldNotCreateCustomerWithInvalidData()
    {
        var request = new CreateCustomerRequest(
            FirstName: "",
            LastName: "Test",
            PhoneNumber: "+380501234567",
            Email: "invalid-email"
        );

        var response = await Client.PostAsJsonAsync(BaseRoute, request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }


    [Fact]
    public async Task ShouldGetCustomerById()
    {
        var customerId = _firstTestCustomer.Id;

        var response = await Client.GetAsync($"{BaseRoute}/{customerId.Value}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var customer = await response.ToResponseModel<CustomerResponseDto>();

        customer.Id.Should().Be(_firstTestCustomer.Id.Value);
        customer.Email.Should().Be(_firstTestCustomer.Email);
    }

    [Fact]
    public async Task ShouldUpdateCustomer()
    {
        var request = new UpdateCustomerRequest(
            FirstName: "UpdatedName",
            LastName: "UpdatedLastName",
            PhoneNumber: "+380632145678",
            Email: "updated.email@example.com"
        );

        var response = await Client.PutAsJsonAsync($"{BaseRoute}/{_firstTestCustomer.Id.Value}", request);

        response.IsSuccessStatusCode.Should().BeTrue();

        var customerDb = await Context.Customers
            .AsNoTracking()
            .FirstAsync(x => x.Id == _firstTestCustomer.Id);

        customerDb.FirstName.Should().Be("UpdatedName");
        customerDb.Email.Should().Be("updated.email@example.com");
    }

    [Fact]
    public async Task ShouldDeleteCustomer()
    {
        var response = await Client.DeleteAsync($"{BaseRoute}/{_secondTestCustomer.Id.Value}");

        response.IsSuccessStatusCode.Should().BeTrue();

        var exists = await Context.Customers
            .AnyAsync(x => x.Id == _secondTestCustomer.Id);

        exists.Should().BeFalse();
    }

    public async Task InitializeAsync()
    {
        _firstTestCustomer = CustomerData.FirstTestCustomer();
        _secondTestCustomer = CustomerData.SecondTestCustomer();

        await Context.Customers.AddAsync(_firstTestCustomer);
        await Context.Customers.AddAsync(_secondTestCustomer);

        await SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        Context.Customers.RemoveRange(Context.Customers);
        await SaveChangesAsync();
    }
}
