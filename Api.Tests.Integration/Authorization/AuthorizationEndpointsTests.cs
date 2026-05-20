using API.Dtos;
using Domain.Customers;
using Domain.Instruments;
using FluentAssertions;
using Domain.Enums;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Tests.Common;
using Tests.Data.Customers;
using Tests.Data.Instruments;
using Xunit;

namespace Api.Tests.Integration.Authorization;

public class AuthorizationEndpointsTests : BaseIntegrationTest, IAsyncLifetime
{
    private const string AuthRoute = "api/auth";

    public AuthorizationEndpointsTests(IntegrationTestWebFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task ProtectedEndpoint_WithoutToken_ReturnsUnauthorized()
    {
        var anonymousClient = Factory.CreateClient();
        var response = await anonymousClient.GetAsync("api/instruments");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task RegisterAndLogin_Customer_CanReadOwnInstrumentsOnly()
    {
        var email = $"customer-{Guid.NewGuid():N}@test.com";
        const string password = "Password1";

        var registerResponse = await Client.PostAsJsonAsync(
            $"{AuthRoute}/register",
            new RegisterDto(email, password, "Test", "Customer"));
        registerResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var loginResponse = await Client.PostAsJsonAsync(
            $"{AuthRoute}/login",
            new LoginDto(email, password));
        loginResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var auth = await loginResponse.ToResponseModel<AuthResponseDto>();
        var customerClient = Factory.CreateClient();
        customerClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", auth.Token);

        var ownInstrument = Instrument.New(
            InstrumentId.New(),
            "Guitar",
            "SN-001",
            DateTime.UtcNow,
            InstrumentStatus.Ready,
            (await Context.Customers.FirstAsync(c => c.Email == email)).Id);

        await Context.Instruments.AddAsync(ownInstrument);

        var otherCustomer = CustomerData.SecondTestCustomer();
        var otherInstrument = Instrument.New(
            InstrumentId.New(),
            "Other",
            "SN-002",
            DateTime.UtcNow,
            InstrumentStatus.Ready,
            otherCustomer.Id);
        await Context.Customers.AddAsync(otherCustomer);
        await Context.Instruments.AddAsync(otherInstrument);
        await SaveChangesAsync();

        var listResponse = await customerClient.GetAsync("api/instruments");
        listResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var foreignResponse = await customerClient.GetAsync($"api/instruments/{otherInstrument.Id.Value}");
        foreignResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var meResponse = await customerClient.GetAsync("api/customers/me");
        meResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Customer_CannotCreateInstrument_ReturnsForbidden()
    {
        var (customerClient, customer) = await CreateAuthenticatedCustomerClientAsync();

        var request = InstrumentData.CreateValidRequest(customerId: customer.Id.Value);
        var response = await customerClient.PostAsJsonAsync("api/instruments", request);

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Master_CanCreateCustomer()
    {
        var request = CustomerData.CreateValidRequest();
        var response = await Client.PostAsJsonAsync("api/customers", request);
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    private async Task<(HttpClient Client, Customer Customer)> CreateAuthenticatedCustomerClientAsync()
    {
        var email = $"customer-{Guid.NewGuid():N}@test.com";
        const string password = "Password1";

        await Client.PostAsJsonAsync(
            $"{AuthRoute}/register",
            new RegisterDto(email, password, "Test", "User"));

        var loginResponse = await Client.PostAsJsonAsync(
            $"{AuthRoute}/login",
            new LoginDto(email, password));

        var auth = await loginResponse.ToResponseModel<AuthResponseDto>();
        var customer = await Context.Customers.FirstAsync(c => c.Email == email);

        var customerClient = Factory.CreateClient();
        customerClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", auth.Token);

        return (customerClient, customer);
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        Context.RepairOrderServiceTypes.RemoveRange(Context.RepairOrderServiceTypes);
        Context.RepairOrders.RemoveRange(Context.RepairOrders);
        Context.InstrumentPassports.RemoveRange(Context.InstrumentPassports);
        Context.Instruments.RemoveRange(Context.Instruments);
        Context.Customers.RemoveRange(Context.Customers);

        var userManager = Factory.Services.CreateScope().ServiceProvider
            .GetRequiredService<UserManager<ApplicationUser>>();
        foreach (var user in await userManager.Users.ToListAsync())
            await userManager.DeleteAsync(user);

        await SaveChangesAsync();
    }
}
