using Application.Common;
using Application.Common.Interfaces.Repositories;
using Domain.Customers;
using Domain.Instruments;
using MediatR;

namespace Application.Instruments.Commands
{
    public class CreateInstrumentCommandHandler : IRequestHandler<CreateInstrumentCommand, Result<InstrumentId>>
    {
        private readonly IInstrumentRepository _repository;

        public CreateInstrumentCommandHandler(IInstrumentRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<InstrumentId>> Handle(CreateInstrumentCommand request, CancellationToken cancellationToken)
        {
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
