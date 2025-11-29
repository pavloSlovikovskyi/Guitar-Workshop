using Application.Common;
using Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Instruments.Commands
{
    public record UpdateInstrumentStatusCommand(
        Guid Id,
        InstrumentStatus Status
    ) : IRequest<Result>;

}
