using Application.Common;
using Application.Common.Interfaces.Queries;
using Domain.RepairOrders;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.RepairOrders.Queries
{
    public class GetAllRepairOrdersQueryHandler : IRequestHandler<GetAllRepairOrdersQuery, Result<List<RepairOrder>>>
    {
        private readonly IRepairOrderQueries _queries;

        public GetAllRepairOrdersQueryHandler(IRepairOrderQueries queries)
        {
            _queries = queries;
        }

        public async Task<Result<List<RepairOrder>>> Handle(GetAllRepairOrdersQuery request, CancellationToken cancellationToken)
        {
            var orders = await _queries.GetAllAsync(cancellationToken);
            return Result<List<RepairOrder>>.Success(orders.ToList());
        }
    }
}
