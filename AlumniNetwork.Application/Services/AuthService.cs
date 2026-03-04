using AlumniNetwork.Application.Common;
using AlumniNetwork.Application.DTOs;
using AlumniNetwork.Application.Interfaces;
using AlumniNetwork.Domain.Entities;
using AlumniNetwork.Domain.Enums;

namespace AlumniNetwork.Application.Services;

public class AuthService(
    IUserRepository userRepository,
    IJwtTokenService jwtTokenService,
    IPasswordHasher passwordHasher,
    IUnitOfWork unitOfWork) : IAuthService
{
    public async Task<ApiResponse<AuthResponseDto>> RegisterAsync(RegisterRequestDto request, CancellationToken cancellationToken = default)
    {
        var existing = await userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (existing is not null)
        {
            return ApiResponse<AuthResponseDto>.Fail("Email already registered.");
        }

        if (!Enum.TryParse<UserRole>(request.Role, true, out var role))
        {
            role = UserRole.Alumni;
        }

        var user = new User
        {
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            Email = request.Email.Trim().ToLowerInvariant(),
            PasswordHash = passwordHasher.Hash(request.Password),
            Role = role,
            Department = request.Department.Trim(),
            YearOfJoining = request.YearOfJoining,
            CurrentLocation = request.CurrentLocation.Trim(),
            CurrentCompany = request.CurrentCompany.Trim(),
            CreatedAt = DateTime.UtcNow
        };

        await userRepository.AddAsync(user, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var (token, expiresAt) = jwtTokenService.GenerateToken(user);
        var response = new AuthResponseDto(token, expiresAt, user.ToProfileDto());

        return ApiResponse<AuthResponseDto>.Ok(response, "User registered successfully.");
    }

    public async Task<ApiResponse<AuthResponseDto>> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByEmailAsync(request.Email.Trim(), cancellationToken);
        if (user is null || !passwordHasher.Verify(request.Password, user.PasswordHash))
        {
            return ApiResponse<AuthResponseDto>.Fail("Invalid email or password.");
        }

        var (token, expiresAt) = jwtTokenService.GenerateToken(user);
        var response = new AuthResponseDto(token, expiresAt, user.ToProfileDto());

        return ApiResponse<AuthResponseDto>.Ok(response, "Login successful.");
    }
}
