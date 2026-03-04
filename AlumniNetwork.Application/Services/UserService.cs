using AlumniNetwork.Application.Common;
using AlumniNetwork.Application.DTOs;
using AlumniNetwork.Application.Interfaces;

namespace AlumniNetwork.Application.Services;

public class UserService(IUserRepository userRepository, IUnitOfWork unitOfWork) : IUserService
{
    public async Task<ApiResponse<UserProfileDto>> GetProfileAsync(int userId, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
        {
            return ApiResponse<UserProfileDto>.Fail("User not found.");
        }

        return ApiResponse<UserProfileDto>.Ok(user.ToProfileDto());
    }

    public async Task<ApiResponse<UserProfileDto>> UpdateProfileAsync(int userId, UpdateProfileRequestDto request, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
        {
            return ApiResponse<UserProfileDto>.Fail("User not found.");
        }

        user.FirstName = request.FirstName.Trim();
        user.LastName = request.LastName.Trim();
        user.Department = request.Department.Trim();
        user.YearOfJoining = request.YearOfJoining;
        user.CurrentLocation = request.CurrentLocation.Trim();
        user.CurrentCompany = request.CurrentCompany.Trim();

        userRepository.Update(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ApiResponse<UserProfileDto>.Ok(user.ToProfileDto(), "Profile updated successfully.");
    }

    public async Task<ApiResponse<PagedResponse<UserProfileDto>>> SearchAlumniAsync(AlumniSearchQueryDto query, CancellationToken cancellationToken = default)
    {
        var result = await userRepository.SearchAsync(query, cancellationToken);
        var response = new PagedResponse<UserProfileDto>
        {
            TotalCount = result.TotalCount,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize,
            Data = result.Data.Select(x => x.ToProfileDto()).ToArray()
        };

        return ApiResponse<PagedResponse<UserProfileDto>>.Ok(response);
    }
}
