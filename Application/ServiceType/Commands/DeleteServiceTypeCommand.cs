using Application.Common;
using Domain.ServiceTypes;
using MediatR;

namespace Application.ServiceTypes.Commands
{
    public record DeleteServiceTypeCommand(ServiceTypeId Id) : IRequest<Result>;
}
