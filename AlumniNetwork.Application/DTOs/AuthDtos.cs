namespace AlumniNetwork.Application.DTOs;

public record RegisterRequestDto(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    string Role,
    string Department,
    int YearOfJoining,
    string CurrentLocation,
    string CurrentCompany);

public record LoginRequestDto(string Email, string Password);

public record AuthResponseDto(string Token, DateTime ExpiresAt, UserProfileDto User);
