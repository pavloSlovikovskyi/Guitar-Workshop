using Application.Common.Interfaces.Repositories;
using Domain.Instruments;
using MediatR;
using Application.Common;

namespace Application.Instruments.Commands;

public class CreateInstrumentCommandHandler : IRequestHandler<CreateInstrumentCommand, Result<Guid>>
{
    private readonly IInstrumentRepository _repository;

    public CreateInstrumentCommandHandler(IInstrumentRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<Guid>> Handle(CreateInstrumentCommand request, CancellationToken cancellationToken)
    {
        var instrument = Instrument.New(
            Guid.NewGuid(),
            request.Model,
            request.SerialNumber,
            request.RecieveDate,
            request.Status,
            request.CustomerId ?? Guid.Empty
        );

        await _repository.AddAsync(instrument, cancellationToken);

        return Result<Guid>.Success(instrument.Id);
    }
}
