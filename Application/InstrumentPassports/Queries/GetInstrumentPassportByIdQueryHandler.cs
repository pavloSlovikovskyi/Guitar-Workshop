using Application.Common;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Queries;
using Domain.InstrumentPassports;
using MediatR;

namespace Application.InstrumentPassports.Queries;

public class GetInstrumentPassportByIdQueryHandler
    : IRequestHandler<GetInstrumentPassportByIdQuery, Result<InstrumentPassport>>
{
    private readonly IInstrumentPassportQueries _queries;
    private readonly ICurrentUserService _currentUser;

    public GetInstrumentPassportByIdQueryHandler(
        IInstrumentPassportQueries queries,
        ICurrentUserService currentUser)
    {
        _queries = queries;
        _currentUser = currentUser;
    }

    public async Task<Result<InstrumentPassport>> Handle(
        GetInstrumentPassportByIdQuery request,
        CancellationToken cancellationToken)
    {
        var passport = await _queries.GetByIdWithInstrumentAsync(request.Id, cancellationToken);
        if (passport is null)
            return Result<InstrumentPassport>.Failure(AuthorizationErrors.NotFound);

        if (!_currentUser.IsMaster)
        {
            var (guard, customerId) = await AccessGuard.RequireCustomerIdAsync(_currentUser, cancellationToken);
            if (!guard.IsSuccess)
                return Result<InstrumentPassport>.Failure(guard.Error!);

            if (passport.Instrument is null || passport.Instrument.CustomerId != customerId)
                return Result<InstrumentPassport>.Failure(AuthorizationErrors.NotFound);
        }

        return Result<InstrumentPassport>.Success(passport);
    }
}
