using Application.Common;
using Application.Common.Interfaces.Repositories;
using Domain.ServiceTypes;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.ServiceTypes.Commands
{
    public class UpdateServiceTypeCommandHandler : IRequestHandler<UpdateServiceTypeCommand, Result>
    {
        private readonly IServiceTypeRepository _repository;

        public UpdateServiceTypeCommandHandler(IServiceTypeRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result> Handle(UpdateServiceTypeCommand request, CancellationToken cancellationToken)
        {
            var serviceType = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (serviceType == null)
                return Result.Failure("ServiceType not found");

            serviceType.UpdateDetails(request.Title, request.Description, request.Price);

            await _repository.UpdateAsync(serviceType, cancellationToken);

            return Result.Success();
        }
    }
}
