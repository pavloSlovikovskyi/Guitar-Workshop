using Application.Common;
using Application.Common.Interfaces;
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
        private readonly ICurrentUserService _currentUser;

        public CreateServiceTypeCommandHandler(
            IServiceTypeRepository repository,
            ICurrentUserService currentUser)
        {
            _repository = repository;
            _currentUser = currentUser;
        }

        public async Task<Result<ServiceTypeId>> Handle(CreateServiceTypeCommand request, CancellationToken cancellationToken)
        {
            var guard = AccessGuard.EnsureMaster(_currentUser);
            if (!guard.IsSuccess)
                return Result<ServiceTypeId>.Failure(guard.Error!);

            var serviceType = ServiceType.New(ServiceTypeId.New(), request.Title, request.Description, request.Price);

            await _repository.AddAsync(serviceType, cancellationToken);

            return Result<ServiceTypeId>.Success(serviceType.Id);
        }
    }
}
