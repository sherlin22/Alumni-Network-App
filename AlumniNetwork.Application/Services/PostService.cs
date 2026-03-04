using AlumniNetwork.Application.Common;
using AlumniNetwork.Application.DTOs;
using AlumniNetwork.Application.Interfaces;
using AlumniNetwork.Domain.Entities;

namespace AlumniNetwork.Application.Services;

public class PostService(
    IUserRepository userRepository,
    IPostRepository postRepository,
    ILikeRepository likeRepository,
    ICommentRepository commentRepository,
    IUnitOfWork unitOfWork) : IPostService
{
    public async Task<ApiResponse<PostResponseDto>> CreatePostAsync(int userId, CreatePostRequestDto request, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
        {
            return ApiResponse<PostResponseDto>.Fail("User not found.");
        }

        var post = new Post
        {
            UserId = userId,
            Content = request.Content.Trim(),
            ImageUrl = string.IsNullOrWhiteSpace(request.ImageUrl) ? null : request.ImageUrl.Trim(),
            CreatedAt = DateTime.UtcNow
        };

        await postRepository.AddAsync(post, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        post.User = user;
        return ApiResponse<PostResponseDto>.Ok(post.ToPostDto(), "Post created successfully.");
    }

    public async Task<ApiResponse<PagedResponse<PostResponseDto>>> GetPostsAsync(PostQueryDto query, CancellationToken cancellationToken = default)
    {
        var result = await postRepository.GetPagedAsync(query, cancellationToken);
        var response = new PagedResponse<PostResponseDto>
        {
            TotalCount = result.TotalCount,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize,
            Data = result.Data.Select(x => x.ToPostDto()).ToArray()
        };

        return ApiResponse<PagedResponse<PostResponseDto>>.Ok(response);
    }

    public async Task<ApiResponse<string>> LikePostAsync(int userId, int postId, CancellationToken cancellationToken = default)
    {
        var post = await postRepository.GetByIdAsync(postId, cancellationToken);
        if (post is null)
        {
            return ApiResponse<string>.Fail("Post not found.");
        }

        var user = await userRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
        {
            return ApiResponse<string>.Fail("User not found.");
        }

        var existingLike = await likeRepository.GetAsync(postId, userId, cancellationToken);
        if (existingLike is not null)
        {
            return ApiResponse<string>.Ok("Already liked.", "No-op.");
        }

        await likeRepository.AddAsync(new Like { PostId = postId, UserId = userId }, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ApiResponse<string>.Ok("Liked", "Post liked successfully.");
    }

    public async Task<ApiResponse<CommentResponseDto>> AddCommentAsync(int userId, int postId, AddCommentRequestDto request, CancellationToken cancellationToken = default)
    {
        var post = await postRepository.GetByIdAsync(postId, cancellationToken);
        if (post is null)
        {
            return ApiResponse<CommentResponseDto>.Fail("Post not found.");
        }

        var user = await userRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
        {
            return ApiResponse<CommentResponseDto>.Fail("User not found.");
        }

        var comment = new Comment
        {
            PostId = postId,
            UserId = userId,
            CommentText = request.CommentText.Trim(),
            CreatedAt = DateTime.UtcNow,
            User = user
        };

        await commentRepository.AddAsync(comment, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ApiResponse<CommentResponseDto>.Ok(comment.ToCommentDto(), "Comment added successfully.");
    }

    public async Task<ApiResponse<IReadOnlyCollection<CommentResponseDto>>> GetCommentsAsync(int postId, CancellationToken cancellationToken = default)
    {
        var comments = await commentRepository.GetByPostIdAsync(postId, cancellationToken);
        var response = comments.Select(x => x.ToCommentDto()).ToArray();

        return ApiResponse<IReadOnlyCollection<CommentResponseDto>>.Ok(response);
    }

    public async Task<ApiResponse<string>> ModerateDeletePostAsync(int postId, CancellationToken cancellationToken = default)
    {
        var post = await postRepository.GetByIdAsync(postId, cancellationToken);
        if (post is null)
        {
            return ApiResponse<string>.Fail("Post not found.");
        }

        postRepository.Delete(post);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ApiResponse<string>.Ok("Deleted", "Post deleted successfully.");
    }
}
