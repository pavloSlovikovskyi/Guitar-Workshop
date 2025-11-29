using Application.Common;
using Application.Common.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.RepairOrdersServiceTypes.Commands
{
    public class AddServiceToRepairOrderCommandHandler : IRequestHandler<AddServiceToRepairOrderCommand, Result>
    {
        private readonly IRepairOrderServiceTypeRepository _repository;

        public AddServiceToRepairOrderCommandHandler(IRepairOrderServiceTypeRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result> Handle(AddServiceToRepairOrderCommand request, CancellationToken cancellationToken)
        {
            var exists = await _repository.ExistsAsync(request.OrderId, request.ServiceId, cancellationToken);
            if (exists)
                return Result.Failure("Service already added to order");

            var entity = new Domain.RepairOrdersServiceTypes.RepairOrderServiceType(request.OrderId, request.ServiceId);
            await _repository.AddAsync(entity, cancellationToken);

            return Result.Success();
        }
    }

   
}
