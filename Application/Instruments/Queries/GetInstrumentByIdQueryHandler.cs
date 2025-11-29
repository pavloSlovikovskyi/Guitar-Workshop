using Application.Common;
using Application.Common.Interfaces.Queries;
using Domain.Instruments;
using MediatR;

namespace Application.Instruments.Queries;

public class GetInstrumentByIdQueryHandler : IRequestHandler<GetInstrumentByIdQuery, Result<Instrument>>
{
    private readonly IInstrumentQueries _queries;

    public GetInstrumentByIdQueryHandler(IInstrumentQueries queries)
    {
        _queries = queries;
    }

    public async Task<Result<Instrument>> Handle(GetInstrumentByIdQuery request, CancellationToken cancellationToken)
    {
        var instrument = await _queries.GetByIdAsync(request.Id, cancellationToken);
        if (instrument == null)
            return Result<Instrument>.Failure("Instrument not found");
        return Result<Instrument>.Success(instrument);
    }
}
