using Application.Common;
using Domain.InstrumentPassports;
using MediatR;

namespace Application.InstrumentPassports.Queries
{
    public record GetInstrumentPassportByIdQuery(InstrumentPassportId Id) : IRequest<Result<InstrumentPassport>>;
}
