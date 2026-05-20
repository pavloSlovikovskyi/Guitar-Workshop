using Application.Common;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;
using Domain.RepairOrdersServiceTypes;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.RepairOrdersServiceTypes.Commands
{
    public class RemoveServiceFromRepairOrderCommandHandler : IRequestHandler<RemoveServiceFromRepairOrderCommand, Result>
    {
        private readonly IRepairOrderServiceTypeRepository _repository;
        private readonly ICurrentUserService _currentUser;

        public RemoveServiceFromRepairOrderCommandHandler(
            IRepairOrderServiceTypeRepository repository,
            ICurrentUserService currentUser)
        {
            _repository = repository;
            _currentUser = currentUser;
        }

        public async Task<Result> Handle(RemoveServiceFromRepairOrderCommand request, CancellationToken cancellationToken)
        {
            var guard = AccessGuard.EnsureMaster(_currentUser);
            if (!guard.IsSuccess)
                return guard;

            var entity = await _repository.GetByOrderAndServiceIdAsync(request.OrderId, request.ServiceId, cancellationToken);
            if (entity == null)
                return Result.Failure("Service not found for this order");

            await _repository.DeleteAsync(entity, cancellationToken);

            return Result.Success();
        }

    }
}
