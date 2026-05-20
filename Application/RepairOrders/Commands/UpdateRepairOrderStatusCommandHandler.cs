using Application.Common;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;
using Domain.RepairOrders;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.RepairOrders.Commands
{
    public class UpdateRepairOrderStatusCommandHandler : IRequestHandler<UpdateRepairOrderStatusCommand, Result>
    {
        private readonly IRepairOrderRepository _repository;
        private readonly ICurrentUserService _currentUser;

        public UpdateRepairOrderStatusCommandHandler(
            IRepairOrderRepository repository,
            ICurrentUserService currentUser)
        {
            _repository = repository;
            _currentUser = currentUser;
        }

        public async Task<Result> Handle(UpdateRepairOrderStatusCommand request, CancellationToken cancellationToken)
        {
            var guard = AccessGuard.EnsureMaster(_currentUser);
            if (!guard.IsSuccess)
                return guard;

            var order = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (order == null)
                return Result.Failure("Repair order not found");

            order.UpdateStatus(request.Status);

            await _repository.UpdateAsync(order, cancellationToken);

            return Result.Success();
        }
    }
}
