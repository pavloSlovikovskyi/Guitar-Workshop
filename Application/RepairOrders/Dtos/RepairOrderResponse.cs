using Domain.Enums;

namespace Application.RepairOrders.Dtos;

public record RepairOrderResponse(
    Guid Id,
    Guid InstrumentId,
    string InstrumentName,
    DateTime OrderDate,
    RepairOrderStatus Status,
    string Notes,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    List<ServiceTypeResponse> Services
);
