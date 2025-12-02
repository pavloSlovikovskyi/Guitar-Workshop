using Application.Common.Interfaces.Repositories;
using Domain.Instruments;
using MediatR;
using Application.Common;

namespace Application.Instruments.Commands
{
    public class UpdateInstrumentCommandHandler : IRequestHandler<UpdateInstrumentCommand, Result>
    {
        private readonly IInstrumentRepository _repository;

        public UpdateInstrumentCommandHandler(IInstrumentRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result> Handle(UpdateInstrumentCommand request, CancellationToken cancellationToken)
        {
            var instrument = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (instrument is null)
                return Result.Failure("Instrument not found");

            instrument.UpdateDetails(
                request.Model,
                request.SerialNumber,
                request.RecieveDate
            );

            await _repository.UpdateAsync(instrument, cancellationToken);

            return Result.Success();
        }
    }
}
