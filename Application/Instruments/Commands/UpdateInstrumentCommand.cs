using Domain.Instruments;
using MediatR;
using Application.Common;

namespace Application.Instruments.Commands
{
    public record UpdateInstrumentCommand(
        InstrumentId Id,
        string Model,
        string SerialNumber,
        DateTime RecieveDate
    ) : IRequest<Result>;
}
