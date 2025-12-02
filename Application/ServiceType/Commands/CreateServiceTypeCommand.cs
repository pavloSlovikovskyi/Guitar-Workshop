using Application.Common;
using Domain.ServiceTypes;
using MediatR;

namespace Application.ServiceTypes.Commands
{
    public record CreateServiceTypeCommand(
        string Title,
        string Description,
        decimal Price
    ) : IRequest<Result<ServiceTypeId>>;
}
