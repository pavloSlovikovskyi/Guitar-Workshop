using Application.Common;
using Application.Common.Interfaces.Repositories;
using Domain.Instruments;
using MediatR;

namespace Application.Instruments.Commands
{
    public class DeleteInstrumentCommandHandler : IRequestHandler<DeleteInstrumentCommand, Result>
    {
        private readonly IInstrumentRepository _repository;

        public DeleteInstrumentCommandHandler(IInstrumentRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result> Handle(DeleteInstrumentCommand request, CancellationToken cancellationToken)
        {
            var instrument = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (instrument == null)
                return Result.Failure("Instrument not found");

            await _repository.DeleteAsync(instrument, cancellationToken);

            return Result.Success();
        }
    }
}
