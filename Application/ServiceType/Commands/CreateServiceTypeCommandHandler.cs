using Application.Common;
using Application.Common.Interfaces.Repositories;
using Domain.ServiceTypes;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.ServiceTypes.Commands
{
    public class CreateServiceTypeCommandHandler : IRequestHandler<CreateServiceTypeCommand, Result<ServiceTypeId>>
    {
        private readonly IServiceTypeRepository _repository;

        public CreateServiceTypeCommandHandler(IServiceTypeRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<ServiceTypeId>> Handle(CreateServiceTypeCommand request, CancellationToken cancellationToken)
        {
            var serviceType = ServiceType.New(ServiceTypeId.New(), request.Title, request.Description, request.Price);

            await _repository.AddAsync(serviceType, cancellationToken);

            return Result<ServiceTypeId>.Success(serviceType.Id);
        }
    }
}
