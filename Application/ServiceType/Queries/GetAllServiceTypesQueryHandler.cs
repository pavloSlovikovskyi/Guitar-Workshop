using Application.Common;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.ServiceTypes;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.ServiceTypes.Queries;

public class GetAllServiceTypesQueryHandler : IRequestHandler<GetAllServiceTypesQuery, Result<List<ServiceType>>>
{
    private readonly IServiceTypeRepository _repository;

    public GetAllServiceTypesQueryHandler(IServiceTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<List<ServiceType>>> Handle(GetAllServiceTypesQuery request, CancellationToken cancellationToken)
    {
        var items = await _repository.GetAllAsync(cancellationToken);
        return Result<List<ServiceType>>.Success(items.ToList());
    }
}

