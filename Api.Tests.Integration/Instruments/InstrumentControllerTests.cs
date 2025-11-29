using System.Net;
using System.Net.Http.Json;
using API.Dtos;
using Domain.Instruments;
using Domain.Enums;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Tests.Common;
using Xunit;
using Tests.Data.Instruments;

namespace Api.Tests.Integration;

public class InstrumentControllerTests : BaseIntegrationTest, IAsyncLifetime
{
    private readonly Instrument _firstTestInstrument = InstrumentData.FirstTestInstrument();
    private readonly Instrument _secondTestInstrument = InstrumentData.SecondTestInstrument();

    private const string BaseRoute = "api/instruments";

    public InstrumentControllerTests(IntegrationTestWebFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task ShouldCreateInstrument()
    {
        // Arrange
        var request = InstrumentData.CreateValidRequest();

        // Act
        var response = await Client.PostAsJsonAsync(BaseRoute, request);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var result = await response.ToResponseModel<dynamic>();
        Guid instrumentId = result.id;
        instrumentId.Should().NotBeEmpty();

        var instrumentDb = await Context.Instruments.FirstAsync(x => x.Id == instrumentId);
        instrumentDb.SerialNumber.Should().Be(request.SerialNumber);//зразок
    }


    [Fact]
    public async Task ShouldNotCreateInstrumentWithInvalidData()
    {
        // Arrange
        var request = new CreateInstrumentRequest(
            Model: "",
            SerialNumber: "SN-123",
            RecieveDate: DateTime.UtcNow,
            Status: InstrumentStatus.Ready,
            CustomerId: Guid.NewGuid()
        );

        // Act
        var response = await Client.PostAsJsonAsync(BaseRoute, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ShouldGetAllInstruments()
    {
        // Arrange

        // Act
        var response = await Client.GetAsync(BaseRoute);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var instruments = await response.ToResponseModel<List<InstrumentResponse>>();
        instruments.Should().HaveCount(2);
    }

    [Fact]
    public async Task ShouldGetInstrumentById()
    {
        // Arrange
        var instrumentId = _firstTestInstrument.Id;

        // Act
        var response = await Client.GetAsync($"{BaseRoute}/{instrumentId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var instrument = await response.ToResponseModel<InstrumentResponse>();
        instrument.Id.Should().Be(instrumentId);
        instrument.Model.Should().Be(_firstTestInstrument.Model);
        instrument.SerialNumber.Should().Be(_firstTestInstrument.SerialNumber);
    }

    [Fact]
    public async Task ShouldReturnNotFoundWhenInstrumentDoesNotExist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var response = await Client.GetAsync($"{BaseRoute}/{nonExistentId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ShouldUpdateInstrument()
    {
        // Arrange
        var request = new UpdateInstrumentRequest(
            Model: "Updated Model Name",
            SerialNumber: "SN-UPDATED",
            RecieveDate: DateTime.UtcNow
        );

        // Act
        var response = await Client.PutAsJsonAsync($"{BaseRoute}/{_firstTestInstrument.Id}", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var instrumentDb = await Context.Instruments
            .AsNoTracking()
            .FirstAsync(x => x.Id.Equals(_firstTestInstrument.Id));

        instrumentDb.Model.Should().Be("Updated Model Name");
        instrumentDb.SerialNumber.Should().Be("SN-UPDATED");
    }

    [Fact]
    public async Task ShouldNotUpdateNonExistentInstrument()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();
        var request = new UpdateInstrumentRequest(
            Model: "Test",
            SerialNumber: "SN-123",
            RecieveDate: DateTime.UtcNow
        );

        // Act
        var response = await Client.PutAsJsonAsync($"{BaseRoute}/{nonExistentId}", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ShouldUpdateInstrumentStatus()
    {
        // Arrange
        var request = new UpdateInstrumentStatusRequest(InstrumentStatus.InRepair);

        // Act
        var response = await Client.PatchAsJsonAsync($"{BaseRoute}/{_firstTestInstrument.Id}/status", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var instrumentDb = await Context.Instruments
            .AsNoTracking()
            .FirstAsync(x => x.Id.Equals(_firstTestInstrument.Id));

        instrumentDb.Status.Should().Be(InstrumentStatus.InRepair);
    }

    [Fact]
    public async Task ShouldDeleteInstrument()
    {
        // Act
        var response = await Client.DeleteAsync($"{BaseRoute}/{_firstTestInstrument.Id}");

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var instrumentExists = await Context.Instruments
            .AnyAsync(x => x.Id.Equals(_firstTestInstrument.Id));

        instrumentExists.Should().BeFalse();
    }

    public async Task InitializeAsync()
    {
        await Context.Instruments.AddAsync(_firstTestInstrument);
        await Context.Instruments.AddAsync(_secondTestInstrument);
        await SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        Context.Instruments.RemoveRange(Context.Instruments);
        await SaveChangesAsync();
    }
}
