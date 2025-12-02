using Application.Common;
using Domain.InstrumentPassports;
using MediatR;
using System;

namespace Application.InstrumentPassports.Commands
{
    public record UpdateInstrumentPassportCommand(
        InstrumentPassportId Id,
        DateTime IssueDate,
        string Details
    ) : IRequest<Result>;
}
