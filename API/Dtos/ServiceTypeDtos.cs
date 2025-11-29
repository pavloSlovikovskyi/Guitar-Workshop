namespace API.Dtos
{
    public record CreateServiceTypeRequest(
        string Title,
        string Description,
        decimal Price
    );

    public record UpdateServiceTypeRequest(
        string Title,
        string Description,
        decimal Price
    );

    public record ServiceTypeResponse(
        Guid Id,
        string Title,
        string Description,
        decimal Price
    );
}
