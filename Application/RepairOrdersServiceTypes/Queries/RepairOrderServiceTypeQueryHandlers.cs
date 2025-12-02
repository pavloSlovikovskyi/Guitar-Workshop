using Application.Common;
using Application.Common.Interfaces.Repositories;
using Domain.RepairOrdersServiceTypes;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.RepairOrdersServiceTypes.Queries
{
    public class GetServicesByOrderIdQueryHandler : IRequestHandler<GetServicesByOrderIdQuery, Result<List<RepairOrderServiceType>>>
    {
        private readonly IRepairOrderServiceTypeRepository _repository;

        public GetServicesByOrderIdQueryHandler(IRepairOrderServiceTypeRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<List<RepairOrderServiceType>>> Handle(GetServicesByOrderIdQuery request, CancellationToken cancellationToken)
        {
            var list = await _repository.GetByOrderIdAsync(request.OrderId, cancellationToken);
            return Result<List<RepairOrderServiceType>>.Success(list.ToList());
        }
    }

    public class GetOrdersByServiceIdQueryHandler : IRequestHandler<GetOrdersByServiceIdQuery, Result<List<RepairOrderServiceType>>>
    {
        private readonly IRepairOrderServiceTypeRepository _repository;

        public GetOrdersByServiceIdQueryHandler(IRepairOrderServiceTypeRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<List<RepairOrderServiceType>>> Handle(GetOrdersByServiceIdQuery request, CancellationToken cancellationToken)
        {
            var list = await _repository.GetByServiceIdAsync(request.ServiceId, cancellationToken);
            return Result<List<RepairOrderServiceType>>.Success(list.ToList());
        }
    }
}
