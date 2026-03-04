using AlumniNetwork.Application.Common;
using AlumniNetwork.Application.DTOs;

namespace AlumniNetwork.Application.Interfaces;

public interface IAuthService
{
    Task<ApiResponse<AuthResponseDto>> RegisterAsync(RegisterRequestDto request, CancellationToken cancellationToken = default);
    Task<ApiResponse<AuthResponseDto>> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default);
}

public interface IUserService
{
    Task<ApiResponse<UserProfileDto>> GetProfileAsync(int userId, CancellationToken cancellationToken = default);
    Task<ApiResponse<UserProfileDto>> UpdateProfileAsync(int userId, UpdateProfileRequestDto request, CancellationToken cancellationToken = default);
    Task<ApiResponse<PagedResponse<UserProfileDto>>> SearchAlumniAsync(AlumniSearchQueryDto query, CancellationToken cancellationToken = default);
}

public interface IPostService
{
    Task<ApiResponse<PostResponseDto>> CreatePostAsync(int userId, CreatePostRequestDto request, CancellationToken cancellationToken = default);
    Task<ApiResponse<PagedResponse<PostResponseDto>>> GetPostsAsync(PostQueryDto query, CancellationToken cancellationToken = default);
    Task<ApiResponse<string>> LikePostAsync(int userId, int postId, CancellationToken cancellationToken = default);
    Task<ApiResponse<CommentResponseDto>> AddCommentAsync(int userId, int postId, AddCommentRequestDto request, CancellationToken cancellationToken = default);
    Task<ApiResponse<IReadOnlyCollection<CommentResponseDto>>> GetCommentsAsync(int postId, CancellationToken cancellationToken = default);
    Task<ApiResponse<string>> ModerateDeletePostAsync(int postId, CancellationToken cancellationToken = default);
}

public interface IAdminService
{
    Task<ApiResponse<PagedResponse<UserProfileDto>>> GetAllUsersAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task<ApiResponse<string>> DeleteUserAsync(int userId, CancellationToken cancellationToken = default);
}
