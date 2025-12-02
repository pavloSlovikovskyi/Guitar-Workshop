using Application.Common;
using Domain.Enums;
using Domain.Instruments;
using MediatR;

namespace Application.Instruments.Commands
{
    public record UpdateInstrumentStatusCommand(
        InstrumentId Id,
        InstrumentStatus Status
    ) : IRequest<Result>;
}
