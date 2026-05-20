using Application.Common;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;
using Domain.Instruments;
using Domain.RepairOrders;
using MediatR;

namespace Application.RepairOrders.Commands
{
    public class CreateRepairOrderCommandHandler : IRequestHandler<CreateRepairOrderCommand, Result<RepairOrderId>>
    {
        private readonly IRepairOrderRepository _repository;
        private readonly ICurrentUserService _currentUser;

        public CreateRepairOrderCommandHandler(
            IRepairOrderRepository repository,
            ICurrentUserService currentUser)
        {
            _repository = repository;
            _currentUser = currentUser;
        }

        public async Task<Result<RepairOrderId>> Handle(CreateRepairOrderCommand request, CancellationToken cancellationToken)
        {
            var guard = AccessGuard.EnsureMaster(_currentUser);
            if (!guard.IsSuccess)
                return Result<RepairOrderId>.Failure(guard.Error!);

            var repairOrder = RepairOrder.New(
                RepairOrderId.New(),
                request.InstrumentId,
                request.OrderDate,
                request.Status,
                request.Notes
            );

            await _repository.AddAsync(repairOrder, cancellationToken);

            return Result<RepairOrderId>.Success(repairOrder.Id);
        }
    }
}
