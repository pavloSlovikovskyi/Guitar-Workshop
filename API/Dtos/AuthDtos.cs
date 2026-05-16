namespace API.Dtos;

public record RegisterDto(
    string Email,
    string Password,
    string FirstName,
    string LastName);

public record LoginDto(
    string Email,
    string Password);

public record AuthResponseDto(
    string Token,
    DateTime ExpiresAtUtc);

public record RegisterResponseDto(Guid CustomerId);
