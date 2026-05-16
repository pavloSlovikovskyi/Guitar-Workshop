namespace Application.Common.Interfaces;

public interface IJwtTokenService
{
    AuthTokenResult GenerateToken(string userId, string email, IEnumerable<string> roles);
}
