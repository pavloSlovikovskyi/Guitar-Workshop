using Application.Common;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Queries;
using Domain.Instruments;
using MediatR;

namespace Application.Instruments.Queries;

public class GetAllInstrumentsQueryHandler : IRequestHandler<GetAllInstrumentsQuery, Result<List<Instrument>>>
{
    private readonly IInstrumentQueries _queries;
    private readonly ICurrentUserService _currentUser;

    public GetAllInstrumentsQueryHandler(IInstrumentQueries queries, ICurrentUserService currentUser)
    {
        _queries = queries;
        _currentUser = currentUser;
    }

    public async Task<Result<List<Instrument>>> Handle(
        GetAllInstrumentsQuery request,
        CancellationToken cancellationToken)
    {
        IEnumerable<Instrument> instruments;

        if (_currentUser.IsMaster)
        {
            instruments = await _queries.GetAllAsync(cancellationToken);
        }
        else
        {
            var (guard, customerId) = await AccessGuard.RequireCustomerIdAsync(_currentUser, cancellationToken);
            if (!guard.IsSuccess)
                return Result<List<Instrument>>.Failure(guard.Error!);

            instruments = await _queries.GetAllByCustomerIdAsync(customerId!, cancellationToken);
        }

        return Result<List<Instrument>>.Success(instruments.ToList());
    }
}
