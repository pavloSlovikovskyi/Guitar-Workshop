using Application.Common;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Queries;
using Domain.RepairOrders;
using MediatR;

namespace Application.RepairOrders.Queries;

public class GetRepairOrderByIdQueryHandler : IRequestHandler<GetRepairOrderByIdQuery, Result<RepairOrder>>
{
    private readonly IRepairOrderQueries _queries;
    private readonly ICurrentUserService _currentUser;

    public GetRepairOrderByIdQueryHandler(IRepairOrderQueries queries, ICurrentUserService currentUser)
    {
        _queries = queries;
        _currentUser = currentUser;
    }

    public async Task<Result<RepairOrder>> Handle(
        GetRepairOrderByIdQuery request,
        CancellationToken cancellationToken)
    {
        var order = await _queries.GetByIdWithInstrumentAsync(request.Id, cancellationToken);
        if (order is null)
            return Result<RepairOrder>.Failure(AuthorizationErrors.NotFound);

        if (!_currentUser.IsMaster)
        {
            var (guard, customerId) = await AccessGuard.RequireCustomerIdAsync(_currentUser, cancellationToken);
            if (!guard.IsSuccess)
                return Result<RepairOrder>.Failure(guard.Error!);

            if (order.Instrument is null || order.Instrument.CustomerId != customerId)
                return Result<RepairOrder>.Failure(AuthorizationErrors.NotFound);
        }

        return Result<RepairOrder>.Success(order);
    }
}
