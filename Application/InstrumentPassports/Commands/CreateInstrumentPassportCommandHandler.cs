using Application.Common;
using Application.Common.Interfaces.Repositories;
using Domain.InstrumentPassports;
using Domain.Instruments;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.InstrumentPassports.Commands
{
    public class CreateInstrumentPassportCommandHandler : IRequestHandler<CreateInstrumentPassportCommand, Result<InstrumentPassportId>>
    {
        private readonly IInstrumentPassportRepository _repository;

        public CreateInstrumentPassportCommandHandler(IInstrumentPassportRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<InstrumentPassportId>> Handle(CreateInstrumentPassportCommand request, CancellationToken cancellationToken)
        {
            var passport = InstrumentPassport.New(
                InstrumentPassportId.New(),
                request.InstrumentId,
                request.IssueDate,
                request.Details
            );

            await _repository.AddAsync(passport, cancellationToken);

            return Result<InstrumentPassportId>.Success(passport.Id);
        }
    }
}
