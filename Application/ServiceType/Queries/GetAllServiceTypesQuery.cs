using Application.Common;
using MediatR;
using System.Collections.Generic;
using Domain.ServiceTypes;

namespace Application.ServiceTypes.Queries;

public record GetAllServiceTypesQuery() : IRequest<Result<List<ServiceType>>>;
