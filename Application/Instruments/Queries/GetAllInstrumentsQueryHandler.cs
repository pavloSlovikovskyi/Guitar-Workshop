using Application.Common;
using Application.Common.Interfaces.Queries;
using Domain.Instruments;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Instruments.Queries
{
    public class GetAllInstrumentsQueryHandler : IRequestHandler<GetAllInstrumentsQuery, Result<List<Instrument>>>
    {
        private readonly IInstrumentQueries _queries;

        public GetAllInstrumentsQueryHandler(IInstrumentQueries queries)
        {
            _queries = queries;
        }

        public async Task<Result<List<Instrument>>> Handle(GetAllInstrumentsQuery request, CancellationToken cancellationToken)
        {
            var instruments = await _queries.GetAllAsync(cancellationToken);
            if (instruments == null)
                return Result<List<Instrument>>.Failure("Instruments not found");
            return Result<List<Instrument>>.Success(instruments.ToList());
        }
    }
}
