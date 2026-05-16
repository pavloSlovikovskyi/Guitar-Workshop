namespace Application.Common.Interfaces;

public interface IAuthAccountService
{
    Task<AuthRegistrationResult> RegisterAsync(
        string email,
        string password,
        string firstName,
        string lastName,
        CancellationToken cancellationToken = default);

    Task<AuthenticatedUser?> ValidateLoginAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default);
}

public sealed record AuthRegistrationResult(
    bool Succeeded,
    Guid? CustomerId,
    IReadOnlyList<string> Errors);

public sealed record AuthenticatedUser(
    string UserId,
    string Email,
    IReadOnlyList<string> Roles);

public sealed record AuthTokenResult(
    string Token,
    DateTime ExpiresAtUtc);
