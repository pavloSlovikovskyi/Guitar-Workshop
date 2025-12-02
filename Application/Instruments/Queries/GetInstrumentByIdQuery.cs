using Application.Common;
using Domain.Instruments;
using MediatR;

namespace Application.Instruments.Queries
{
    public record GetInstrumentByIdQuery(InstrumentId Id) : IRequest<Result<Instrument>>;
}
