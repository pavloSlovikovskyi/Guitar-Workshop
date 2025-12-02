using Application.Common;
using Domain.InstrumentPassports;
using MediatR;

namespace Application.InstrumentPassports.Commands
{
    public record DeleteInstrumentPassportCommand(InstrumentPassportId Id) : IRequest<Result>;
}
