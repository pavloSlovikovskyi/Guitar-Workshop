using Application.Common;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Queries;
using Domain.Instruments;
using MediatR;

namespace Application.Instruments.Queries;

public class GetInstrumentByIdQueryHandler : IRequestHandler<GetInstrumentByIdQuery, Result<Instrument>>
{
    private readonly IInstrumentQueries _queries;
    private readonly ICurrentUserService _currentUser;

    public GetInstrumentByIdQueryHandler(IInstrumentQueries queries, ICurrentUserService currentUser)
    {
        _queries = queries;
        _currentUser = currentUser;
    }

    public async Task<Result<Instrument>> Handle(
        GetInstrumentByIdQuery request,
        CancellationToken cancellationToken)
    {
        var instrument = await _queries.GetByIdAsync(request.Id, cancellationToken);
        if (instrument is null)
            return Result<Instrument>.Failure(AuthorizationErrors.NotFound);

        if (!_currentUser.IsMaster)
        {
            var (guard, customerId) = await AccessGuard.RequireCustomerIdAsync(_currentUser, cancellationToken);
            if (!guard.IsSuccess)
                return Result<Instrument>.Failure(guard.Error!);

            var ownership = AccessGuard.EnsureOwnCustomer(_currentUser, instrument.CustomerId, customerId);
            if (!ownership.IsSuccess)
                return Result<Instrument>.Failure(ownership.Error!);
        }

        return Result<Instrument>.Success(instrument);
    }
}
