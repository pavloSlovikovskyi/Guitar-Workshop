using Application.Common;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;
using Domain.RepairOrders;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.RepairOrders.Commands
{
    public class UpdateRepairOrderCommandHandler : IRequestHandler<UpdateRepairOrderCommand, Result>
    {
        private readonly IRepairOrderRepository _repository;
        private readonly ICurrentUserService _currentUser;

        public UpdateRepairOrderCommandHandler(
            IRepairOrderRepository repository,
            ICurrentUserService currentUser)
        {
            _repository = repository;
            _currentUser = currentUser;
        }

        public async Task<Result> Handle(UpdateRepairOrderCommand request, CancellationToken cancellationToken)
        {
            var guard = AccessGuard.EnsureMaster(_currentUser);
            if (!guard.IsSuccess)
                return guard;

            var repairOrder = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (repairOrder == null)
                return Result.Failure("Repair order not found");

            repairOrder.UpdateDetails(
                request.OrderDate,
                request.Status,
                request.Notes
            );

            await _repository.UpdateAsync(repairOrder, cancellationToken);

            return Result.Success();
        }
    }
}
