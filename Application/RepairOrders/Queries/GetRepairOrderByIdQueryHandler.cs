using Application.Common;
using Application.Common.Interfaces.Queries;
using Domain.RepairOrders;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.RepairOrders.Queries
{
    public class GetRepairOrderByIdQueryHandler : IRequestHandler<GetRepairOrderByIdQuery, Result<RepairOrder>>
    {
        private readonly IRepairOrderQueries _queries;

        public GetRepairOrderByIdQueryHandler(IRepairOrderQueries queries)
        {
            _queries = queries;
        }

        public async Task<Result<RepairOrder>> Handle(GetRepairOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var order = await _queries.GetByIdAsync(request.Id, cancellationToken);
            return order == null
                ? Result<RepairOrder>.Failure("Repair order not found")
                : Result<RepairOrder>.Success(order);
        }
    }
}
