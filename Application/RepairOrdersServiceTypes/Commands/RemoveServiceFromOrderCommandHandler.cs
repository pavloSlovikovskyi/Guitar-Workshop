using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.RepairOrdersServiceTypes.Commands;
using MediatR;

public class RemoveServiceFromRepairOrderCommandHandler : IRequestHandler<RemoveServiceFromRepairOrderCommand, Result>
{
    private readonly IRepairOrderServiceTypeRepository _repository;

    public RemoveServiceFromRepairOrderCommandHandler(IRepairOrderServiceTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result> Handle(RemoveServiceFromRepairOrderCommand request, CancellationToken cancellationToken)
    {
        var exists = await _repository.ExistsAsync(request.OrderId, request.ServiceId, cancellationToken);
        if (!exists)
            return Result.Failure("Service not found for this order");

        var entity = new Domain.RepairOrdersServiceTypes.RepairOrderServiceType(request.OrderId, request.ServiceId);
        await _repository.DeleteAsync(entity, cancellationToken);

        return Result.Success();
    }
}