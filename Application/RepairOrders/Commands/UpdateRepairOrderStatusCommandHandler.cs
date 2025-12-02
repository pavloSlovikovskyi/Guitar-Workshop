using Application.Common;
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

        public UpdateRepairOrderStatusCommandHandler(IRepairOrderRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result> Handle(UpdateRepairOrderStatusCommand request, CancellationToken cancellationToken)
        {
            var order = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (order == null)
                return Result.Failure("Repair order not found");

            order.UpdateStatus(request.Status);

            await _repository.UpdateAsync(order, cancellationToken);

            return Result.Success();
        }
    }
}
