using Domain.RepairOrders;
using Domain.Enums;
using API.Dtos;
using Domain.Instruments;

namespace Tests.Data.RepairOrders;

public static class RepairOrderData
{
    public static RepairOrder FirstTestRepairOrder(InstrumentId instrumentId) =>
        RepairOrder.New(
            RepairOrderId.New(),
            instrumentId,
            DateTime.UtcNow,
            RepairOrderStatus.Open,
            "Initial repair assessment"
        );

    public static RepairOrder SecondTestRepairOrder(InstrumentId instrumentId) =>
        RepairOrder.New(
            RepairOrderId.New(),
            instrumentId,
            DateTime.UtcNow.AddDays(-2),
            RepairOrderStatus.InProgress,
            "Working on repairs"
        );

    public static CreateRepairOrderRequest CreateValidRequest(InstrumentId instrumentId) =>
        new CreateRepairOrderRequest(
            InstrumentId: instrumentId.Value,
            OrderDate: DateTime.UtcNow,
            Status: RepairOrderStatus.Open,
            Notes: "Test repair order"
        );
}
