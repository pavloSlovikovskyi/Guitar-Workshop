using Application.Common;
using Domain.InstrumentPassports;
using Domain.Instruments;
using MediatR;
using System;

namespace Application.InstrumentPassports.Commands
{
    public record CreateInstrumentPassportCommand(
        InstrumentId InstrumentId,
        DateTime IssueDate,
        string Details
    ) : IRequest<Result<InstrumentPassportId>>;
}
