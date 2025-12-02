using Domain.Enums;
using Domain.Instruments;
using Domain.RepairOrders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Common.ResponseModels
{
    public record RepairOrderId([property: JsonProperty("value")] Guid Value);
    public record InstrumentIdDto([property: JsonProperty("value")] Guid Value);
    public record RepairOrderResponseDto(
        RepairOrderId Id,
        InstrumentIdDto InstrumentId,
        DateTime OrderDate,
        RepairOrderStatus Status,
        string Notes,
        DateTime CreatedAt,
        DateTime? UpdatedAt);


}
