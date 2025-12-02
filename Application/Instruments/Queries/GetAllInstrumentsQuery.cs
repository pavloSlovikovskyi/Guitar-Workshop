using Application.Common;
using Domain.Instruments;
using MediatR;
using System.Collections.Generic;

namespace Application.Instruments.Queries
{
    public record GetAllInstrumentsQuery() : IRequest<Result<List<Instrument>>>;
}
