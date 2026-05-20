using Application.Common;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;
using Domain.Customers;
using Domain.Instruments;
using MediatR;

namespace Application.Instruments.Commands
{
    public class CreateInstrumentCommandHandler : IRequestHandler<CreateInstrumentCommand, Result<InstrumentId>>
    {
        private readonly IInstrumentRepository _repository;
        private readonly ICurrentUserService _currentUser;

        public CreateInstrumentCommandHandler(
            IInstrumentRepository repository,
            ICurrentUserService currentUser)
        {
            _repository = repository;
            _currentUser = currentUser;
        }

        public async Task<Result<InstrumentId>> Handle(CreateInstrumentCommand request, CancellationToken cancellationToken)
        {
            var guard = AccessGuard.EnsureMaster(_currentUser);
            if (!guard.IsSuccess)
                return Result<InstrumentId>.Failure(guard.Error!);

            var instrument = Instrument.New(
                InstrumentId.New(),
                request.Model,
                request.SerialNumber,
                request.RecieveDate,
                request.Status,
                request.CustomerId ?? CustomerId.Empty()
            );

            await _repository.AddAsync(instrument, cancellationToken);

            return Result<InstrumentId>.Success(instrument.Id);
        }
    }
}
