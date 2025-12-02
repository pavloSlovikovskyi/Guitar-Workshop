using Application.Common;
using Domain.ServiceTypes;
using MediatR;

namespace Application.ServiceTypes.Commands
{
    public record UpdateServiceTypeCommand(
        ServiceTypeId Id,
        string Title,
        string Description,
        decimal Price
    ) : IRequest<Result>;
}
