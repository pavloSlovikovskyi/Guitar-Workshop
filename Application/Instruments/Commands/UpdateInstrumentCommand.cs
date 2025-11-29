using MediatR;
using Application.Common;

namespace Application.Instruments.Commands;

public record UpdateInstrumentCommand(
    Guid Id,
    string Model,
    string SerialNumber,
    DateTime RecieveDate
) : IRequest<Result>;
