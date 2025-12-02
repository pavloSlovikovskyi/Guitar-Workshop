using Application.Common;
using Domain.InstrumentPassports;
using MediatR;
using System.Collections.Generic;

namespace Application.InstrumentPassports.Queries
{
    public record GetAllInstrumentPassportsQuery() : IRequest<Result<List<InstrumentPassport>>>;
}
