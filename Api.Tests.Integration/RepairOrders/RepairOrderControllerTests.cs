using API.Dtos;
using Domain.Customers;
using Domain.Enums;
using Domain.Instruments;
using Domain.RepairOrders;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http.Json;
using Tests.Common;
using Tests.Data.Customers;
using Tests.Data.Instruments;
using Tests.Data.RepairOrders;
using Tests.Common.ResponseModels;
using TestModel = Tests.Common.ResponseModels;
using Xunit;

namespace Api.Tests.Integration;

public class RepairOrderControllerTests : BaseIntegrationTest, IAsyncLifetime
{
    private RepairOrder _firstTestOrder;
    private RepairOrder _secondTestOrder;
    private readonly Customer _testCustomer = CustomerData.FirstTestCustomer();
    private Instrument _testInstrument;

    private const string BaseRoute = "api/orders";

    public RepairOrderControllerTests(IntegrationTestWebFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task ShouldCreateRepairOrder()
    {
        var request = RepairOrderData.CreateValidRequest(_testInstrument.Id);

        var response = await Client.PostAsJsonAsync(BaseRoute, request);

        response.IsSuccessStatusCode.Should().BeTrue();
    }

    [Fact]
    public async Task ShouldNotCreateOrderWithNonExistentInstrument()
    {
        var request = RepairOrderData.CreateValidRequest(InstrumentId.New());

        var response = await Client.PostAsJsonAsync(BaseRoute, request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ShouldGetAllRepairOrders()
    {
        var response = await Client.GetAsync(BaseRoute);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var orders = await response.ToResponseModel<List<TestModel.RepairOrderResponseDto>>();
        orders.Should().HaveCount(2);
        foreach (var order in orders)
        {
            order.Id.Value.Should().NotBe(Guid.Empty);
            order.InstrumentId.Value.Should().NotBe(Guid.Empty);
        }
    }

    [Fact]
    public async Task ShouldGetRepairOrderById()
    {
        var repairOrderId = _firstTestOrder.Id;

        var response = await Client.GetAsync($"{BaseRoute}/{repairOrderId.Value}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var order = await response.ToResponseModel<TestModel.RepairOrderResponseDto>();

        order.Id.Value.Should().Be(_firstTestOrder.Id.Value);
        order.InstrumentId.Value.Should().Be(_testInstrument.Id.Value);
    }

    [Fact]
    public async Task ShouldUpdateRepairOrder()
    {
        var request = new UpdateRepairOrderRequest(
            InstrumentId: _testInstrument.Id.Value,
            OrderDate: DateTime.UtcNow,
            Status: RepairOrderStatus.Completed,
            Notes: "Repair completed successfully"
        );

        var response = await Client.PutAsJsonAsync($"{BaseRoute}/{_firstTestOrder.Id.Value}", request);

        response.IsSuccessStatusCode.Should().BeTrue();

        var orderDb = await Context.RepairOrders
            .AsNoTracking()
            .FirstAsync(x => x.Id == _firstTestOrder.Id);

        orderDb.Status.Should().Be(RepairOrderStatus.Completed);
        orderDb.Notes.Should().Be("Repair completed successfully");
    }

    [Fact]
    public async Task ShouldUpdateRepairOrderStatus()
    {
        var request = new UpdateRepairOrderStatusRequest(RepairOrderStatus.InProgress);

        var response = await Client.PatchAsJsonAsync($"{BaseRoute}/{_firstTestOrder.Id.Value}/status", request);

        response.IsSuccessStatusCode.Should().BeTrue();

        var orderDb = await Context.RepairOrders
            .AsNoTracking()
            .FirstAsync(x => x.Id == _firstTestOrder.Id);

        orderDb.Status.Should().Be(RepairOrderStatus.InProgress);
    }

    public async Task InitializeAsync()
    {
        await Context.Customers.AddAsync(_testCustomer);

        _testInstrument = InstrumentData.CreateInstrumentWithCustomerId(_testCustomer.Id);
        await Context.Instruments.AddAsync(_testInstrument);

        _firstTestOrder = RepairOrderData.FirstTestRepairOrder(_testInstrument.Id);
        _secondTestOrder = RepairOrderData.SecondTestRepairOrder(_testInstrument.Id);
        await Context.RepairOrders.AddAsync(_firstTestOrder);
        await Context.RepairOrders.AddAsync(_secondTestOrder);

        await SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        Context.RepairOrders.RemoveRange(Context.RepairOrders);
        Context.Instruments.RemoveRange(Context.Instruments);
        Context.Customers.RemoveRange(Context.Customers);
        await SaveChangesAsync();
    }
}
