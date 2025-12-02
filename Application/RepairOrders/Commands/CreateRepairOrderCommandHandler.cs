using Application.Common;
using Application.Common.Interfaces.Repositories;
using Domain.Instruments;
using Domain.RepairOrders;
using MediatR;

namespace Application.RepairOrders.Commands
{
    public class CreateRepairOrderCommandHandler : IRequestHandler<CreateRepairOrderCommand, Result<RepairOrderId>>
    {
        private readonly IRepairOrderRepository _repository;

        public CreateRepairOrderCommandHandler(IRepairOrderRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<RepairOrderId>> Handle(CreateRepairOrderCommand request, CancellationToken cancellationToken)
        {
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
