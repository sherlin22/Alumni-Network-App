namespace AlumniNetwork.Application.DTOs;

public record UserProfileDto(
    int Id,
    string FirstName,
    string LastName,
    string Email,
    string Role,
    string Department,
    int YearOfJoining,
    string CurrentLocation,
    string CurrentCompany,
    DateTime CreatedAt);

public record UpdateProfileRequestDto(
    string FirstName,
    string LastName,
    string Department,
    int YearOfJoining,
    string CurrentLocation,
    string CurrentCompany);

public record AlumniSearchQueryDto(string? Department, int? Year, string? Location, int PageNumber = 1, int PageSize = 10);
