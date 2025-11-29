using Domain.Enums;
using MediatR;
using Application.Common;

namespace Application.Instruments.Commands;

public record CreateInstrumentCommand(
    string Model,
    string SerialNumber,
    DateTime RecieveDate,
    InstrumentStatus Status,
    Guid? CustomerId
) : IRequest<Result<Guid>>;
