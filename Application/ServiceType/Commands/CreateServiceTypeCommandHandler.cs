using Application.Common;
using Application.Common.Interfaces.Repositories;
using Domain.ServiceTypes;
using MediatR;

namespace Application.ServiceTypes.Commands;

public class CreateServiceTypeCommandHandler : IRequestHandler<CreateServiceTypeCommand, Result<Guid>>
{
    private readonly IServiceTypeRepository _repository;

    public CreateServiceTypeCommandHandler(IServiceTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<Guid>> Handle(CreateServiceTypeCommand request, CancellationToken cancellationToken)
    {
        var serviceType = ServiceType.New(Guid.NewGuid(), request.Title, request.Description, request.Price);

        await _repository.AddAsync(serviceType, cancellationToken);

        return Result<Guid>.Success(serviceType.Id);
    }
}
