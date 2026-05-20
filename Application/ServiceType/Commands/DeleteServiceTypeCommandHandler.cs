using Application.Common;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;
using Domain.ServiceTypes;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.ServiceTypes.Commands
{
    public class DeleteServiceTypeCommandHandler : IRequestHandler<DeleteServiceTypeCommand, Result>
    {
        private readonly IServiceTypeRepository _repository;
        private readonly ICurrentUserService _currentUser;

        public DeleteServiceTypeCommandHandler(
            IServiceTypeRepository repository,
            ICurrentUserService currentUser)
        {
            _repository = repository;
            _currentUser = currentUser;
        }

        public async Task<Result> Handle(DeleteServiceTypeCommand request, CancellationToken cancellationToken)
        {
            var guard = AccessGuard.EnsureMaster(_currentUser);
            if (!guard.IsSuccess)
                return guard;

            var serviceType = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (serviceType == null)
                return Result.Failure("ServiceType not found");

            await _repository.DeleteAsync(serviceType, cancellationToken);

            return Result.Success();
        }
    }
}
