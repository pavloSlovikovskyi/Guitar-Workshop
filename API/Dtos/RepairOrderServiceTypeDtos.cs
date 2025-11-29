namespace API.Dtos
{
    public record RepairOrderServiceTypeResponse(
        Guid OrderId,
        Guid ServiceId
    );

    public record AddServiceToOrderRequest(Guid ServiceId);

    public record RemoveServiceFromOrderRequest(Guid ServiceId);
}
