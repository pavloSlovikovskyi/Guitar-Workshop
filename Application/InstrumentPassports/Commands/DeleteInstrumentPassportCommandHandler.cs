using Application.Common;
using Application.Common.Interfaces.Repositories;
using Domain.InstrumentPassports;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.InstrumentPassports.Commands
{
    public class DeleteInstrumentPassportCommandHandler : IRequestHandler<DeleteInstrumentPassportCommand, Result>
    {
        private readonly IInstrumentPassportRepository _repository;

        public DeleteInstrumentPassportCommandHandler(IInstrumentPassportRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result> Handle(DeleteInstrumentPassportCommand request, CancellationToken cancellationToken)
        {
            var passport = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (passport == null)
                return Result.Failure("Instrument passport not found");

            await _repository.DeleteAsync(passport, cancellationToken);

            return Result.Success();
        }
    }
}
