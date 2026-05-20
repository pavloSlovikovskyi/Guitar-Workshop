using Application.Common;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;
using Domain.Instruments;
using MediatR;

namespace Application.Instruments.Commands
{
    public class UpdateInstrumentStatusCommandHandler : IRequestHandler<UpdateInstrumentStatusCommand, Result>
    {
        private readonly IInstrumentRepository _repository;
        private readonly ICurrentUserService _currentUser;

        public UpdateInstrumentStatusCommandHandler(
            IInstrumentRepository repository,
            ICurrentUserService currentUser)
        {
            _repository = repository;
            _currentUser = currentUser;
        }

        public async Task<Result> Handle(UpdateInstrumentStatusCommand request, CancellationToken cancellationToken)
        {
            var guard = AccessGuard.EnsureMaster(_currentUser);
            if (!guard.IsSuccess)
                return guard;

            var instrument = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (instrument == null)
                return Result.Failure("Instrument not found");

            instrument.UpdateStatus(request.Status);
            await _repository.UpdateAsync(instrument, cancellationToken);

            return Result.Success();
        }
    }
}
