using Domain.Enums;
using Newtonsoft.Json;
using System;

namespace API.Dtos.Responses
{

    public record InstrumentResponseDto(
        Guid Id,
        string Model,
        string SerialNumber,
        DateTime RecieveDate,
        InstrumentStatus Status);
}
