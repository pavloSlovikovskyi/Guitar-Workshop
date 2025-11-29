using Application.Common;
using Application.Common.Interfaces.Repositories;
using Domain.RepairOrders;
using MediatR;

namespace Application.RepairOrders.Commands;

public class CreateRepairOrderCommandHandler : IRequestHandler<CreateRepairOrderCommand, Result<Guid>>
{
    private readonly IRepairOrderRepository _repository;

    public CreateRepairOrderCommandHandler(IRepairOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<Guid>> Handle(CreateRepairOrderCommand request, CancellationToken cancellationToken)
    {
        var repairOrder = RepairOrder.New(
            Guid.NewGuid(),
            request.InstrumentId,
            request.OrderDate,
            request.Status,
            request.Notes
        );

        await _repository.AddAsync(repairOrder, cancellationToken);

        return Result<Guid>.Success(repairOrder.Id);
    }
}
