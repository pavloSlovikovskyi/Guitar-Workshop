using Domain.Instruments;
using Application.Common;
using MediatR;

namespace Application.Instruments.Commands
{
    public record DeleteInstrumentCommand(InstrumentId Id) : IRequest<Result>;
}
