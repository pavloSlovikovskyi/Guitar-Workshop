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
            var entity = await _repository.GetByOrderAndServiceIdAsync(request.OrderId, request.ServiceId, cancellationToken);
            if (entity == null)
                return Result.Failure("Service not found for this order");

            await _repository.DeleteAsync(entity, cancellationToken);

            return Result.Success();
        }

    }
}
