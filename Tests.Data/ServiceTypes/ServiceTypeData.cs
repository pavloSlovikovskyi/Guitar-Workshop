using Domain.ServiceTypes;
using API.Dtos;

namespace Tests.Data.ServiceTypes;

public static class ServiceTypeData
{
    public static ServiceType FirstTestServiceType() =>
        ServiceType.New(
            Guid.NewGuid(),
            "Guitar Setup",
            "Complete guitar setup and adjustment",
            50.00m
        );

    public static ServiceType SecondTestServiceType() =>
        ServiceType.New(
            Guid.NewGuid(),
            "Fret Leveling",
            "Professional fret leveling and polishing",
            120.00m
        );

    public static CreateServiceTypeRequest CreateValidRequest(
        string? title = null,
        decimal? price = null) =>
        new CreateServiceTypeRequest(
            Title: title ?? "String Replacement",
            Description: "Replace old strings with new ones",
            Price: price ?? 25.00m
        );
}
