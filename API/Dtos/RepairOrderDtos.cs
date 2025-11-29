using Domain.Enums;

namespace API.Dtos
{
    public record CreateRepairOrderRequest(
        Guid InstrumentId,
        DateTime OrderDate,
        RepairOrderStatus Status,
        string Notes
    );

    public record UpdateRepairOrderRequest(
        Guid InstrumentId,
        DateTime OrderDate,
        RepairOrderStatus Status,
        string Notes
    );

    public record UpdateRepairOrderStatusRequest(
        RepairOrderStatus Status
    );

    public record RepairOrderResponse(
        Guid Id,
        Guid InstrumentId,
        DateTime OrderDate,
        RepairOrderStatus Status,
        string Notes,
        DateTime CreatedAt,
        DateTime? UpdatedAt
    );
}
