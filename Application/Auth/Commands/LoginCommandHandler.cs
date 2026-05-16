using Application.Common;
using Application.Common.Interfaces;
using MediatR;

namespace Application.Auth.Commands;

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<AuthTokenResult>>
{
    private readonly IAuthAccountService _authAccountService;
    private readonly IJwtTokenService _jwtTokenService;

    public LoginCommandHandler(
        IAuthAccountService authAccountService,
        IJwtTokenService jwtTokenService)
    {
        _authAccountService = authAccountService;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<Result<AuthTokenResult>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _authAccountService.ValidateLoginAsync(
            request.Email,
            request.Password,
            cancellationToken);

        if (user is null)
            return Result<AuthTokenResult>.Failure("Invalid email or password.");

        var token = _jwtTokenService.GenerateToken(user.UserId, user.Email, user.Roles);
        return Result<AuthTokenResult>.Success(token);
    }
}
