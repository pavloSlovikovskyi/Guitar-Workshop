using Application.Common;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;
using Domain.Instruments;
using MediatR;

namespace Application.Instruments.Commands
{
    public class DeleteInstrumentCommandHandler : IRequestHandler<DeleteInstrumentCommand, Result>
    {
        private readonly IInstrumentRepository _repository;
        private readonly ICurrentUserService _currentUser;

        public DeleteInstrumentCommandHandler(
            IInstrumentRepository repository,
            ICurrentUserService currentUser)
        {
            _repository = repository;
            _currentUser = currentUser;
        }

        public async Task<Result> Handle(DeleteInstrumentCommand request, CancellationToken cancellationToken)
        {
            var guard = AccessGuard.EnsureMaster(_currentUser);
            if (!guard.IsSuccess)
                return guard;

            var instrument = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (instrument == null)
                return Result.Failure("Instrument not found");

            await _repository.DeleteAsync(instrument, cancellationToken);

            return Result.Success();
        }
    }
}
