using Application.Common;
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

        public RemoveServiceFromRepairOrderCommandHandler(IRepairOrderServiceTypeRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result> Handle(RemoveServiceFromRepairOrderCommand request, CancellationToken cancellationToken)
        {
            var exists = await _repository.ExistsAsync(request.OrderId, request.ServiceId, cancellationToken);
            if (!exists)
                return Result.Failure("Service not found for this order");

            var entity = RepairOrderServiceType.New(request.OrderId, request.ServiceId);
            await _repository.DeleteAsync(entity, cancellationToken);

            return Result.Success();
        }
    }
}
