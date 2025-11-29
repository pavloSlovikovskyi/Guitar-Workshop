using Application.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ServiceTypes.Commands
{
    public record DeleteServiceTypeCommand(Guid Id) : IRequest<Result>;
}
