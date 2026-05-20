using Application.Common;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Queries;
using Application.RepairOrders.Dtos;
using MediatR;

namespace Application.RepairOrders.Queries;

public class GetAllRepairOrdersQueryHandler : IRequestHandler<GetAllRepairOrdersQuery, Result<List<RepairOrderResponse>>>
{
    private readonly IRepairOrderQueries _queries;
    private readonly ICurrentUserService _currentUser;

    public GetAllRepairOrdersQueryHandler(IRepairOrderQueries queries, ICurrentUserService currentUser)
    {
        _queries = queries;
        _currentUser = currentUser;
    }

    public async Task<Result<List<RepairOrderResponse>>> Handle(
        GetAllRepairOrdersQuery request,
        CancellationToken cancellationToken)
    {
        List<Domain.RepairOrders.RepairOrder> orders;

        if (_currentUser.IsMaster)
        {
            orders = await _queries.GetAllWithIncludesAsync(cancellationToken);
        }
        else
        {
            var (guard, customerId) = await AccessGuard.RequireCustomerIdAsync(_currentUser, cancellationToken);
            if (!guard.IsSuccess)
                return Result<List<RepairOrderResponse>>.Failure(guard.Error!);

            orders = await _queries.GetAllWithIncludesByCustomerIdAsync(customerId!, cancellationToken);
        }

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
