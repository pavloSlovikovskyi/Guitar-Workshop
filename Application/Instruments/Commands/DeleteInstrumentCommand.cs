using Application.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Instruments.Commands
{
    public record DeleteInstrumentCommand(Guid Id) : IRequest<Result>;
}
