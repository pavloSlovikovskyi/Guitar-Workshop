using Application.Common;
using Application.Common.Interfaces.Repositories;
using Domain.InstrumentPassports;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.InstrumentPassports.Commands
{
    public class UpdateInstrumentPassportCommandHandler : IRequestHandler<UpdateInstrumentPassportCommand, Result>
    {
        private readonly IInstrumentPassportRepository _repository;

        public UpdateInstrumentPassportCommandHandler(IInstrumentPassportRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result> Handle(UpdateInstrumentPassportCommand request, CancellationToken cancellationToken)
        {
            var passport = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (passport == null)
                return Result.Failure("Instrument passport not found");

            passport.UpdateDetails(request.IssueDate, request.Details);

            await _repository.UpdateAsync(passport, cancellationToken);

            return Result.Success();
        }
    }
}
