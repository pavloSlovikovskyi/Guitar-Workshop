using Application.Common;
using Application.Common.Interfaces.Queries;
using Domain.InstrumentPassports;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.InstrumentPassports.Queries
{
    public class GetInstrumentPassportByIdQueryHandler : IRequestHandler<GetInstrumentPassportByIdQuery, Result<InstrumentPassport>>
    {
        private readonly IInstrumentPassportQueries _queries;

        public GetInstrumentPassportByIdQueryHandler(IInstrumentPassportQueries queries)
        {
            _queries = queries;
        }

        public async Task<Result<InstrumentPassport>> Handle(GetInstrumentPassportByIdQuery request, CancellationToken cancellationToken)
        {
            var passport = await _queries.GetByIdAsync(request.Id, cancellationToken);
            if (passport == null)
                return Result<InstrumentPassport>.Failure("Instrument passport not found");
            return Result<InstrumentPassport>.Success(passport);
        }
    }
}
