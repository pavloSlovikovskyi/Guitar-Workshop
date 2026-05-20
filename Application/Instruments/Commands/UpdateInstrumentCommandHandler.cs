using Application.Common;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;
using Domain.Instruments;
using MediatR;

namespace Application.Instruments.Commands
{
    public class UpdateInstrumentCommandHandler : IRequestHandler<UpdateInstrumentCommand, Result>
    {
        private readonly IInstrumentRepository _repository;
        private readonly ICurrentUserService _currentUser;

        public UpdateInstrumentCommandHandler(
            IInstrumentRepository repository,
            ICurrentUserService currentUser)
        {
            _repository = repository;
            _currentUser = currentUser;
        }

        public async Task<Result> Handle(UpdateInstrumentCommand request, CancellationToken cancellationToken)
        {
            var guard = AccessGuard.EnsureMaster(_currentUser);
            if (!guard.IsSuccess)
                return guard;

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
