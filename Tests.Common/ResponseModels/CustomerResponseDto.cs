public record CustomerResponseDto(
    Guid Id,
    string FirstName,
    string LastName,
    string PhoneNumber,
    string Email,
    DateTime CreatedAt,
    DateTime? UpdatedAt);
