using System.Net;
using System.Net.Http.Json;
using API.Dtos;
using Domain.Instruments;
using Domain.RepairOrders;
using Domain.Enums;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Tests.Common;
using Xunit;
using Tests.Data.Instruments;
using Tests.Data.RepairOrders;

namespace Api.Tests.Integration;

public class RepairOrderControllerTests : BaseIntegrationTest, IAsyncLifetime
{
    private readonly Instrument _testInstrument = InstrumentData.FirstTestInstrument();
    private readonly RepairOrder _firstTestOrder;
    private readonly RepairOrder _secondTestOrder;

    private const string BaseRoute = "api/orders";

    public RepairOrderControllerTests(IntegrationTestWebFactory factory) : base(factory)
    {
        _firstTestOrder = RepairOrderData.FirstTestRepairOrder(_testInstrument.Id);
        _secondTestOrder = RepairOrderData.SecondTestRepairOrder(_testInstrument.Id);
    }

    [Fact]
    public async Task ShouldCreateRepairOrder()
    {
        // Arrange
        var request = RepairOrderData.CreateValidRequest(_testInstrument.Id);

        // Act
        var response = await Client.PostAsJsonAsync(BaseRoute, request);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
    }

    [Fact]
    public async Task ShouldNotCreateOrderWithNonExistentInstrument()
    {
        // Arrange
        var request = RepairOrderData.CreateValidRequest(Guid.NewGuid());

        // Act
        var response = await Client.PostAsJsonAsync(BaseRoute, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ShouldGetAllRepairOrders()
    {
        // Act
        var response = await Client.GetAsync(BaseRoute);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var orders = await response.ToResponseModel<List<RepairOrderResponse>>();
        orders.Should().HaveCount(2);
    }

    [Fact]
    public async Task ShouldGetRepairOrderById()
    {
        // Act
        var response = await Client.GetAsync($"{BaseRoute}/{_firstTestOrder.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var order = await response.ToResponseModel<RepairOrderResponse>();
        order.Id.Should().Be(_firstTestOrder.Id);
        order.InstrumentId.Should().Be(_testInstrument.Id);
    }

    [Fact]
    public async Task ShouldUpdateRepairOrder()
    {
        // Arrange
        var request = new UpdateRepairOrderRequest(
            InstrumentId: _testInstrument.Id,
            OrderDate: DateTime.UtcNow,
            Status: RepairOrderStatus.Completed,
            Notes: "Repair completed successfully"
        );

        // Act
        var response = await Client.PutAsJsonAsync($"{BaseRoute}/{_firstTestOrder.Id}", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var orderDb = await Context.RepairOrders
            .AsNoTracking()
            .FirstAsync(x => x.Id.Equals(_firstTestOrder.Id));

        orderDb.Status.Should().Be(RepairOrderStatus.Completed);
        orderDb.Notes.Should().Be("Repair completed successfully");
    }

    [Fact]
    public async Task ShouldUpdateRepairOrderStatus()
    {
        // Arrange
        var request = new UpdateRepairOrderStatusRequest(RepairOrderStatus.InProgress);

        // Act
        var response = await Client.PatchAsJsonAsync($"{BaseRoute}/{_firstTestOrder.Id}/status", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var orderDb = await Context.RepairOrders
            .AsNoTracking()
            .FirstAsync(x => x.Id.Equals(_firstTestOrder.Id));

        orderDb.Status.Should().Be(RepairOrderStatus.InProgress);
    }

    public async Task InitializeAsync()
    {
        await Context.Instruments.AddAsync(_testInstrument);
        await Context.RepairOrders.AddAsync(_firstTestOrder);
        await Context.RepairOrders.AddAsync(_secondTestOrder);
        await SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        Context.RepairOrders.RemoveRange(Context.RepairOrders);
        Context.Instruments.RemoveRange(Context.Instruments);
        await SaveChangesAsync();
    }
}
