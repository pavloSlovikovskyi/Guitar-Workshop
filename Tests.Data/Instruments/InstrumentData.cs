using Domain.Instruments;
using Domain.Enums;
using API.Dtos;

namespace Tests.Data.Instruments;

public static class InstrumentData
{
    public static Instrument FirstTestInstrument() =>
        Instrument.New(
            Guid.NewGuid(),
            "Fender Stratocaster",
            "SN-12345",
            DateTime.UtcNow.AddDays(-10),
            InstrumentStatus.Ready,
            Guid.NewGuid()
        );

    public static Instrument SecondTestInstrument() =>
        Instrument.New(
            Guid.NewGuid(),
            "Gibson Les Paul",
            "SN-67890",
            DateTime.UtcNow.AddDays(-5),
            InstrumentStatus.InRepair,
            Guid.NewGuid()
        );

    public static CreateInstrumentRequest CreateValidRequest(
        string? model = null,
        string? serialNumber = null) =>
        new CreateInstrumentRequest(
            Model: model ?? "Ibanez RG",
            SerialNumber: serialNumber ?? $"SN-{Guid.NewGuid().ToString()[..8]}",
            RecieveDate: DateTime.UtcNow,
            Status: InstrumentStatus.Ready,
            CustomerId: Guid.NewGuid()
        );
}
