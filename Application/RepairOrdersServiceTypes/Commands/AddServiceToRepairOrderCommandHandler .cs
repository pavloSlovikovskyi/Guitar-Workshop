using Application.Common;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;
using Domain.RepairOrdersServiceTypes;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.RepairOrdersServiceTypes.Commands
{
    public class AddServiceToRepairOrderCommandHandler : IRequestHandler<AddServiceToRepairOrderCommand, Result>
    {
        private readonly IRepairOrderServiceTypeRepository _repository;
        private readonly ICurrentUserService _currentUser;

        public AddServiceToRepairOrderCommandHandler(
            IRepairOrderServiceTypeRepository repository,
            ICurrentUserService currentUser)
        {
            _repository = repository;
            _currentUser = currentUser;
        }

        public async Task<Result> Handle(AddServiceToRepairOrderCommand request, CancellationToken cancellationToken)
        {
            var guard = AccessGuard.EnsureMaster(_currentUser);
            if (!guard.IsSuccess)
                return guard;

            var exists = await _repository.ExistsAsync(request.OrderId, request.ServiceId, cancellationToken);
            if (exists)
                return Result.Failure("Service already added to order");

            var entity = RepairOrderServiceType.New(request.OrderId, request.ServiceId);
            await _repository.AddAsync(entity, cancellationToken);

            return Result.Success();
        }
    }
}
