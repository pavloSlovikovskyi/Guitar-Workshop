using Application.Common;
using Application.Common.Interfaces;
using MediatR;

namespace Application.Auth.Commands;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<Guid>>
{
    private readonly IAuthAccountService _authAccountService;

    public RegisterCommandHandler(IAuthAccountService authAccountService)
    {
        _authAccountService = authAccountService;
    }

    public async Task<Result<Guid>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var registration = await _authAccountService.RegisterAsync(
            request.Email,
            request.Password,
            request.FirstName,
            request.LastName,
            cancellationToken);

        if (!registration.Succeeded)
            return Result<Guid>.Failure(registration.Errors);

        return Result<Guid>.Success(registration.CustomerId!.Value);
    }
}
