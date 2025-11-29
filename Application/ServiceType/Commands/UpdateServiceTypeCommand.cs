using MediatR;
using Application.Common;

namespace Application.ServiceTypes.Commands;

public record UpdateServiceTypeCommand(
    Guid Id,
    string Title,
    string Description,
    decimal Price
) : IRequest<Result>;
