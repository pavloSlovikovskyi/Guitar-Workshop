using Application.Common;
using Application.Common.Interfaces.Repositories;
using Domain.ServiceTypes;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.ServiceTypes.Commands
{
    public class DeleteServiceTypeCommandHandler : IRequestHandler<DeleteServiceTypeCommand, Result>
    {
        private readonly IServiceTypeRepository _repository;

        public DeleteServiceTypeCommandHandler(IServiceTypeRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result> Handle(DeleteServiceTypeCommand request, CancellationToken cancellationToken)
        {
            var serviceType = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (serviceType == null)
                return Result.Failure("ServiceType not found");

            await _repository.DeleteAsync(serviceType, cancellationToken);

            return Result.Success();
        }
    }
}
