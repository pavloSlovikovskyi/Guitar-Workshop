using API.Dtos;
using Domain.Customers;
using Domain.Instruments;
using Domain.RepairOrders;
using Domain.RepairOrdersServiceTypes;
using Domain.ServiceTypes;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http.Json;
using Tests.Common;
using Tests.Data.Customers;
using Tests.Data.Instruments;
using Tests.Data.RepairOrders;
using Tests.Data.RepairOrdersServiceTypes;
using Tests.Data.ServiceTypes;
using Xunit;

namespace Api.Tests.Integration;

public class RepairOrderServiceTypeControllerTests : BaseIntegrationTest, IAsyncLifetime
{
    private readonly Customer _testCustomer = CustomerData.FirstTestCustomer();
    private Instrument _testInstrument;
    private RepairOrder _testOrder;
    private readonly ServiceType _firstTestService = ServiceTypeData.FirstTestServiceType();
    private readonly ServiceType _secondTestService = ServiceTypeData.SecondTestServiceType();
    private RepairOrderServiceType _existingLink;

    public RepairOrderServiceTypeControllerTests(IntegrationTestWebFactory factory) : base(factory)
    {
    }

    public async Task InitializeAsync()
    {
        // Додаємо клієнта
        await Context.Customers.AddAsync(_testCustomer);

        // Створюємо інструменти з посиланням на клієнта
        _testInstrument = InstrumentData.CreateInstrumentWithCustomerId(_testCustomer.Id);

        await Context.Instruments.AddAsync(_testInstrument);

        // Додаємо сервіси
        await Context.ServiceTypes.AddAsync(_firstTestService);
        await Context.ServiceTypes.AddAsync(_secondTestService);

        // Створюємо замовлення ремонту з інструментом
        _testOrder = RepairOrderData.FirstTestRepairOrder(_testInstrument.Id);
        await Context.RepairOrders.AddAsync(_testOrder);

        // Створюємо існуючий зв'язок сервісу і замовлення
        _existingLink = RepairOrderServiceTypeData.CreateLink(_testOrder.Id, _firstTestService.Id);
        await Context.RepairOrderServiceTypes.AddAsync(_existingLink);

        await Context.Customers.AddAsync(_testCustomer);

        _testInstrument = InstrumentData.CreateInstrumentWithCustomerId(_testCustomer.Id);
        await Context.Instruments.AddAsync(_testInstrument);

        await SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        Context.RepairOrderServiceTypes.RemoveRange(Context.RepairOrderServiceTypes);
        Context.RepairOrders.RemoveRange(Context.RepairOrders);
        Context.ServiceTypes.RemoveRange(Context.ServiceTypes);
        Context.Instruments.RemoveRange(Context.Instruments);
        Context.Customers.RemoveRange(Context.Customers);
        await SaveChangesAsync();
    }

    [Fact]
    public async Task ShouldAddServiceToOrder()
    {
        var request = new AddServiceToOrderRequest(_secondTestService.Id);

        var response = await Client.PostAsJsonAsync($"api/orders/{_testOrder.Id}/services", request);

        response.IsSuccessStatusCode.Should().BeTrue();

        var links = await Context.RepairOrderServiceTypes
            .Where(x => x.OrderId == _testOrder.Id)
            .ToListAsync();

        links.Should().HaveCount(2);
        links.Should().Contain(x => x.ServiceId == _secondTestService.Id);
    }

    [Fact]
    public async Task ShouldNotAddDuplicateService()
    {
        var request = new AddServiceToOrderRequest(_firstTestService.Id);

        var response = await Client.PostAsJsonAsync($"api/orders/{_testOrder.Id}/services", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ShouldRemoveServiceFromOrder()
    {
        var request = new RemoveServiceFromOrderRequest(_firstTestService.Id);

        var response = await Client.SendAsync(new HttpRequestMessage
        {
            Method = HttpMethod.Delete,
            RequestUri = new Uri($"{Client.BaseAddress}api/orders/{_testOrder.Id}/services"),
            Content = JsonContent.Create(request)
        });

        response.IsSuccessStatusCode.Should().BeTrue();

        var exists = await Context.RepairOrderServiceTypes
            .AnyAsync(x => x.OrderId == _testOrder.Id && x.ServiceId == _firstTestService.Id);

        exists.Should().BeFalse();
    }

    [Fact]
    public async Task ShouldNotRemoveNonExistentService()
    {
        var request = new RemoveServiceFromOrderRequest(_secondTestService.Id);

        var response = await Client.SendAsync(new HttpRequestMessage
        {
            Method = HttpMethod.Delete,
            RequestUri = new Uri($"{Client.BaseAddress}api/orders/{_testOrder.Id}/services"),
            Content = JsonContent.Create(request)
        });

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
