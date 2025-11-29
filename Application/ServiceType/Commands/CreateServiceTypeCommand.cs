using MediatR;
using Application.Common;

namespace Application.ServiceTypes.Commands;

public record CreateServiceTypeCommand(
    string Title,
    string Description,
    decimal Price
) : IRequest<Result<Guid>>;
