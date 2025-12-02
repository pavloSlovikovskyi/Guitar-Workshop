using Application.Common;
using Application.Common.Interfaces.Queries;
using Domain.InstrumentPassports;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.InstrumentPassports.Queries
{
    public class GetAllInstrumentPassportsQueryHandler : IRequestHandler<GetAllInstrumentPassportsQuery, Result<List<InstrumentPassport>>>
    {
        private readonly IInstrumentPassportQueries _queries;

        public GetAllInstrumentPassportsQueryHandler(IInstrumentPassportQueries queries)
        {
            _queries = queries;
        }

        public async Task<Result<List<InstrumentPassport>>> Handle(GetAllInstrumentPassportsQuery request, CancellationToken cancellationToken)
        {
            var passports = await _queries.GetAllAsync(cancellationToken);
            return Result<List<InstrumentPassport>>.Success(passports.ToList());
        }
    }
}
