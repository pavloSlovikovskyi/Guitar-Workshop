using Domain.Enums;
using System.Text.Json.Serialization;

namespace API.Dtos
{
    public record CreateInstrumentRequest(
        string Model,
        string SerialNumber,
        DateTime RecieveDate,
        [property: JsonConverter(typeof(JsonStringEnumConverter))]
        InstrumentStatus Status,
        Guid? CustomerId 
    );

    public record UpdateInstrumentRequest(
        string Model,
        string SerialNumber,
        DateTime RecieveDate
    );

    public record UpdateInstrumentStatusRequest(
        InstrumentStatus Status
    );

    public record InstrumentResponse(
        Guid Id,
        string Model,
        string SerialNumber,
        DateTime RecieveDate,
        InstrumentStatus Status,
        Guid? CustomerId,
        DateTime CreatedAt,
        DateTime? UpdatedAt
    );
}
