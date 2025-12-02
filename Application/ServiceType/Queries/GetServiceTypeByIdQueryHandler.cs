using Application.Common;
using Application.Common.Interfaces.Repositories;
using Domain.ServiceTypes;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.ServiceTypes.Queries
{
    public class GetServiceTypeByIdQueryHandler : IRequestHandler<GetServiceTypeByIdQuery, Result<ServiceType>>
    {
        private readonly IServiceTypeRepository _repository;

        public GetServiceTypeByIdQueryHandler(IServiceTypeRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<ServiceType>> Handle(GetServiceTypeByIdQuery request, CancellationToken cancellationToken)
        {
            var serviceTypeId = new ServiceTypeId(request.Id);

            var item = await _repository.GetByIdAsync(serviceTypeId, cancellationToken);

            return item == null
                ? Result<ServiceType>.Failure("ServiceType not found")
                : Result<ServiceType>.Success(item);
        }

    }
}
