using Application.Common;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Queries;
using Domain.InstrumentPassports;
using MediatR;

namespace Application.InstrumentPassports.Queries;

public class GetAllInstrumentPassportsQueryHandler
    : IRequestHandler<GetAllInstrumentPassportsQuery, Result<List<InstrumentPassport>>>
{
    private readonly IInstrumentPassportQueries _queries;
    private readonly ICurrentUserService _currentUser;

    public GetAllInstrumentPassportsQueryHandler(
        IInstrumentPassportQueries queries,
        ICurrentUserService currentUser)
    {
        _queries = queries;
        _currentUser = currentUser;
    }

    public async Task<Result<List<InstrumentPassport>>> Handle(
        GetAllInstrumentPassportsQuery request,
        CancellationToken cancellationToken)
    {
        IEnumerable<InstrumentPassport> passports;

        if (_currentUser.IsMaster)
        {
            passports = await _queries.GetAllAsync(cancellationToken);
        }
        else
        {
            var (guard, customerId) = await AccessGuard.RequireCustomerIdAsync(_currentUser, cancellationToken);
            if (!guard.IsSuccess)
                return Result<List<InstrumentPassport>>.Failure(guard.Error!);

            passports = await _queries.GetAllByCustomerIdAsync(customerId!, cancellationToken);
        }

        return Result<List<InstrumentPassport>>.Success(passports.ToList());
    }
}
