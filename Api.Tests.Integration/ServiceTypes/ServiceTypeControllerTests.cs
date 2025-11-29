using System.Net;
using System.Net.Http.Json;
using API.Dtos;
using Domain.ServiceTypes;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Tests.Common;
using Tests.Data.ServiceTypes;
using Xunit;

namespace Api.Tests.Integration;

public class ServiceTypeControllerTests : BaseIntegrationTest, IAsyncLifetime
{
    private readonly ServiceType _firstTestServiceType = ServiceTypeData.FirstTestServiceType();
    private readonly ServiceType _secondTestServiceType = ServiceTypeData.SecondTestServiceType();

    private const string BaseRoute = "api/services";

    public ServiceTypeControllerTests(IntegrationTestWebFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task ShouldCreateServiceType()
    {
        // Arrange
        var request = ServiceTypeData.CreateValidRequest();

        // Act
        var response = await Client.PostAsJsonAsync(BaseRoute, request);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        var serviceDto = await response.ToResponseModel<dynamic>();
        ((Guid)serviceDto.id).Should().NotBeEmpty();
    }

    [Fact]
    public async Task ShouldNotCreateServiceTypeWithInvalidPrice()
    {
        // Arrange
        var request = new CreateServiceTypeRequest(
            Title: "Test Service",
            Description: "Description",
            Price: -10.00m
        );

        // Act
        var response = await Client.PostAsJsonAsync(BaseRoute, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ShouldGetAllServiceTypes()
    {
        // Act
        var response = await Client.GetAsync(BaseRoute);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var services = await response.ToResponseModel<List<ServiceTypeResponse>>();
        services.Should().HaveCount(2);
    }

    [Fact]
    public async Task ShouldGetServiceTypeById()
    {
        // Act
        var response = await Client.GetAsync($"{BaseRoute}/{_firstTestServiceType.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var service = await response.ToResponseModel<ServiceTypeResponse>();
        service.Id.Should().Be(_firstTestServiceType.Id);
        service.Title.Should().Be(_firstTestServiceType.Title);
        service.Price.Should().Be(_firstTestServiceType.Price);
    }

    [Fact]
    public async Task ShouldUpdateServiceType()
    {
        // Arrange
        var request = new UpdateServiceTypeRequest(
            Title: "Updated Service",
            Description: "Updated description",
            Price: 99.99m
        );

        // Act
        var response = await Client.PutAsJsonAsync($"{BaseRoute}/{_firstTestServiceType.Id}", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var serviceDb = await Context.ServiceTypes
            .AsNoTracking()
            .FirstAsync(x => x.Id.Equals(_firstTestServiceType.Id));

        serviceDb.Title.Should().Be("Updated Service");
        serviceDb.Price.Should().Be(99.99m);
    }

    [Fact]
    public async Task ShouldDeleteServiceType()
    {
        // Act
        var response = await Client.DeleteAsync($"{BaseRoute}/{_secondTestServiceType.Id}");

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var exists = await Context.ServiceTypes
            .AnyAsync(x => x.Id.Equals(_secondTestServiceType.Id));

        exists.Should().BeFalse();
    }

    public async Task InitializeAsync()
    {
        await Context.ServiceTypes.AddAsync(_firstTestServiceType);
        await Context.ServiceTypes.AddAsync(_secondTestServiceType);
        await SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        Context.ServiceTypes.RemoveRange(Context.ServiceTypes);
        await SaveChangesAsync();
    }
}
