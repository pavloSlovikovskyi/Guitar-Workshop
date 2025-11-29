using Application.Common;
using Domain.ServiceTypes;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ServiceTypes.Queries
{
    public record GetServiceTypeByIdQuery(Guid Id) : IRequest<Result<ServiceType>>;
}
