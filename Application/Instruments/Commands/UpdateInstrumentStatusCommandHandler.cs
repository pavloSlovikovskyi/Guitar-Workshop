using Application.Common;
using Application.Common.Interfaces.Repositories;
using Domain.Instruments;
using MediatR;

namespace Application.Instruments.Commands
{
    public class UpdateInstrumentStatusCommandHandler : IRequestHandler<UpdateInstrumentStatusCommand, Result>
    {
        private readonly IInstrumentRepository _repository;

        public UpdateInstrumentStatusCommandHandler(IInstrumentRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result> Handle(UpdateInstrumentStatusCommand request, CancellationToken cancellationToken)
        {
            var instrument = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (instrument == null)
                return Result.Failure("Instrument not found");

            instrument.UpdateStatus(request.Status);
            await _repository.UpdateAsync(instrument, cancellationToken);

            return Result.Success();
        }
    }
}
