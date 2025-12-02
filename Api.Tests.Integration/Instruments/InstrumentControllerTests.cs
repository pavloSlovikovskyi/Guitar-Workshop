using API.Dtos;
using API.Dtos.Responses;
using Domain.Customers;
using Domain.Enums;
using Domain.Instruments;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using Tests.Common;
using Tests.Data.Customers;
using Tests.Data.Instruments;
using Xunit;

namespace Api.Tests.Integration;

public class InstrumentControllerTests : BaseIntegrationTest, IAsyncLifetime
{
    private Instrument _firstTestInstrument;
    private Instrument _secondTestInstrument;
    private readonly Customer _testCustomer = CustomerData.FirstTestCustomer();

    private const string BaseRoute = "api/instruments";

    public InstrumentControllerTests(IntegrationTestWebFactory factory) : base(factory)
    {
    }

    public record IdDto(Guid id);

    [Fact]
    public async Task ShouldCreateInstrument()
    {
        var request = InstrumentData.CreateValidRequest(customerId: _testCustomer.Id.Value);

        var response = await Client.PostAsJsonAsync(BaseRoute, request);

        response.IsSuccessStatusCode.Should().BeTrue();

        var result = await response.ToResponseModel<IdDto>();
        var instrumentId = result.id;
        instrumentId.Should().NotBe(Guid.Empty);

        var instrumentDb = await Context.Instruments.FirstAsync(x => x.Id == (InstrumentId)instrumentId);

    }


    [Fact]
    public async Task ShouldNotCreateInstrumentWithInvalidData()
    {
        var request = new CreateInstrumentRequest(
            Model: "",
            SerialNumber: "SN-123",
            RecieveDate: DateTime.UtcNow,
            Status: InstrumentStatus.Ready,
            CustomerId: Guid.NewGuid()
        );

        var response = await Client.PostAsJsonAsync(BaseRoute, request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ShouldGetAllInstruments()
    {
        var response = await Client.GetAsync(BaseRoute);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var instruments = await response.ToResponseModel<List<InstrumentResponseDto>>();
        instruments.Should().HaveCount(2);
        foreach (var instrument in instruments)
        {
            instrument.Id.Should().NotBe(Guid.Empty);
        }
    }

    [Fact]
    public async Task ShouldGetInstrumentById()
    {
        var instrumentId = _firstTestInstrument.Id;

        var response = await Client.GetAsync($"{BaseRoute}/{instrumentId.Value}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var instrument = await response.ToResponseModel<InstrumentResponseDto>();

        instrument.Id.Should().Be(_firstTestInstrument.Id.Value);
        instrument.Model.Should().Be(_firstTestInstrument.Model);
        instrument.SerialNumber.Should().Be(_firstTestInstrument.SerialNumber);
    }

    [Fact]
    public async Task ShouldReturnNotFoundWhenInstrumentDoesNotExist()
    {
        var nonExistentId = Guid.NewGuid();

        var response = await Client.GetAsync($"{BaseRoute}/{nonExistentId}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ShouldUpdateInstrument()
    {
        var request = new UpdateInstrumentRequest(
            Model: "Updated Model Name",
            SerialNumber: "SN-UPDATED",
            RecieveDate: DateTime.UtcNow
        );

        var response = await Client.PutAsJsonAsync($"{BaseRoute}/{_firstTestInstrument.Id.Value}", request);

        response.IsSuccessStatusCode.Should().BeTrue();

        var instrumentDb = await Context.Instruments
            .AsNoTracking()
            .FirstAsync(x => x.Id == _firstTestInstrument.Id);

        instrumentDb.Model.Should().Be("Updated Model Name");
        instrumentDb.SerialNumber.Should().Be("SN-UPDATED");
    }

    [Fact]
    public async Task ShouldNotUpdateNonExistentInstrument()
    {
        var nonExistentId = Guid.NewGuid();

        var request = new UpdateInstrumentRequest(
            Model: "Test",
            SerialNumber: "SN-123",
            RecieveDate: DateTime.UtcNow
        );

        var response = await Client.PutAsJsonAsync($"{BaseRoute}/{nonExistentId}", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ShouldUpdateInstrumentStatus()
    {
        var request = new UpdateInstrumentStatusRequest(InstrumentStatus.InRepair);

        var response = await Client.PatchAsJsonAsync($"{BaseRoute}/{_firstTestInstrument.Id.Value}/status", request);

        response.IsSuccessStatusCode.Should().BeTrue();

        var instrumentDb = await Context.Instruments
            .AsNoTracking()
            .FirstAsync(x => x.Id == _firstTestInstrument.Id);

        instrumentDb.Status.Should().Be(InstrumentStatus.InRepair);
    }

    [Fact]
    public async Task ShouldDeleteInstrument()
    {
        var response = await Client.DeleteAsync($"{BaseRoute}/{_firstTestInstrument.Id.Value}");

        response.IsSuccessStatusCode.Should().BeTrue();

        var instrumentExists = await Context.Instruments
            .AnyAsync(x => x.Id == _firstTestInstrument.Id);

        instrumentExists.Should().BeFalse();
    }

    public async Task InitializeAsync()
    {
        await Context.Customers.AddAsync(_testCustomer);

        _firstTestInstrument = InstrumentData.CreateInstrumentWithCustomerId(_testCustomer.Id);
        _secondTestInstrument = InstrumentData.CreateInstrumentWithCustomerId(_testCustomer.Id);

        await Context.Instruments.AddAsync(_firstTestInstrument);
        await Context.Instruments.AddAsync(_secondTestInstrument);

        await SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        Context.Instruments.RemoveRange(Context.Instruments);
        Context.Customers.RemoveRange(Context.Customers);
        await SaveChangesAsync();
    }
}
