using Domain.Instruments;
using Domain.Enums;
using API.Dtos;
using Domain.Customers;

namespace Tests.Data.Instruments;

public static class InstrumentData
{
    public static Instrument FirstTestInstrument() =>
        Instrument.New(
            InstrumentId.New(),
            "Fender Stratocaster",
            "SN-12345",
            DateTime.UtcNow.AddDays(-10),
            InstrumentStatus.Ready,
            CustomerId.New()
        );

    public static Instrument SecondTestInstrument() =>
        Instrument.New(
            InstrumentId.New(),
            "Gibson Les Paul",
            "SN-67890",
            DateTime.UtcNow.AddDays(-5),
            InstrumentStatus.InRepair,
            CustomerId.New()
        );

    public static CreateInstrumentRequest CreateValidRequest(
        string? model = null,
        string? serialNumber = null,
        Guid? customerId = null) =>
        new CreateInstrumentRequest(
            Model: model ?? "Ibanez RG",
            SerialNumber: serialNumber ?? $"SN-{Guid.NewGuid().ToString()[..8]}",
            RecieveDate: DateTime.UtcNow,
            Status: InstrumentStatus.Ready,
            CustomerId: customerId ?? CustomerId.New().Value
        );


    public static Instrument CreateInstrumentWithCustomerId(CustomerId customerId) =>
    Instrument.New(
        InstrumentId.New(),
        "Fender Stratocaster",
        "SN-12345",
        DateTime.UtcNow.AddDays(-10),
        InstrumentStatus.Ready,
        customerId
    );

}
