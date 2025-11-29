using Domain.RepairOrders;
using Domain.Enums;
using API.Dtos;

namespace Tests.Data.RepairOrders;

public static class RepairOrderData
{
    public static RepairOrder FirstTestRepairOrder(Guid instrumentId) =>
        RepairOrder.New(
            Guid.NewGuid(),
            instrumentId,
            DateTime.UtcNow,
            RepairOrderStatus.Open,
            "Initial repair assessment"
        );

    public static RepairOrder SecondTestRepairOrder(Guid instrumentId) =>
        RepairOrder.New(
            Guid.NewGuid(),
            instrumentId,
            DateTime.UtcNow.AddDays(-2),
            RepairOrderStatus.InProgress,
            "Working on repairs"
        );

    public static CreateRepairOrderRequest CreateValidRequest(Guid instrumentId) =>
        new CreateRepairOrderRequest(
            InstrumentId: instrumentId,
            OrderDate: DateTime.UtcNow,
            Status: RepairOrderStatus.Open,
            Notes: "Test repair order"
        );
}
