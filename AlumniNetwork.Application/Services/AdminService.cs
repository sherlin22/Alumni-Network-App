using AlumniNetwork.Application.Common;
using AlumniNetwork.Application.DTOs;
using AlumniNetwork.Application.Interfaces;

namespace AlumniNetwork.Application.Services;

public class AdminService(IUserRepository userRepository, IUnitOfWork unitOfWork) : IAdminService
{
    public async Task<ApiResponse<PagedResponse<UserProfileDto>>> GetAllUsersAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var users = await userRepository.GetAllAsync(pageNumber, pageSize, cancellationToken);

        var response = new PagedResponse<UserProfileDto>
        {
            TotalCount = users.TotalCount,
            PageNumber = users.PageNumber,
            PageSize = users.PageSize,
            Data = users.Data.Select(x => x.ToProfileDto()).ToArray()
        };

        return ApiResponse<PagedResponse<UserProfileDto>>.Ok(response);
    }

    public async Task<ApiResponse<string>> DeleteUserAsync(int userId, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
        {
            return ApiResponse<string>.Fail("User not found.");
        }

        userRepository.Delete(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ApiResponse<string>.Ok("Deleted", "User deleted successfully.");
    }
}
