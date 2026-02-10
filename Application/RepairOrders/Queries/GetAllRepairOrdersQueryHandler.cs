using Application.Common;
using Application.Common.Interfaces.Queries;
using Domain.RepairOrders;
using MediatR;
using System.Linq;
using System.Threading;
using Application.RepairOrders.Dtos;

using System.Threading.Tasks;

namespace Application.RepairOrders.Queries
{
    public class GetAllRepairOrdersQueryHandler : IRequestHandler<GetAllRepairOrdersQuery, Result<List<RepairOrderResponse>>>
    {
        private readonly IRepairOrderQueries _queries;

        public GetAllRepairOrdersQueryHandler(IRepairOrderQueries queries)
        {
            _queries = queries;
        }

        public async Task<Result<List<RepairOrderResponse>>> Handle(
            GetAllRepairOrdersQuery request,
            CancellationToken cancellationToken)
        {
            var orders = await _queries.GetAllWithIncludesAsync(cancellationToken);

            var result = orders.Select(o => new RepairOrderResponse(
                o.Id.Value,
                o.InstrumentId.Value,
                o.OrderDate,
                o.Status,
                o.Notes,
                o.CreatedAt,
                o.UpdatedAt,
                o.RepairOrderServiceTypes
                    .Select(x => new ServiceTypeResponse(
                        x.ServiceType.Id.Value,
                        x.ServiceType.Title
                    )).ToList()
            )).ToList();

            return Result<List<RepairOrderResponse>>.Success(result);
        }

    }
}
