using AlumniNetwork.Application.DTOs;
using AlumniNetwork.Domain.Entities;

namespace AlumniNetwork.Application.Services;

internal static class MappingExtensions
{
    public static UserProfileDto ToProfileDto(this User user) => new(
        user.Id,
        user.FirstName,
        user.LastName,
        user.Email,
        user.Role.ToString(),
        user.Department,
        user.YearOfJoining,
        user.CurrentLocation,
        user.CurrentCompany,
        user.CreatedAt);

    public static PostResponseDto ToPostDto(this Post post) => new(
        post.Id,
        post.UserId,
        $"{post.User.FirstName} {post.User.LastName}",
        post.User.Department,
        post.User.YearOfJoining,
        post.Content,
        post.ImageUrl,
        post.CreatedAt,
        post.Likes.Count,
        post.Comments.Count);

    public static CommentResponseDto ToCommentDto(this Comment comment) => new(
        comment.Id,
        comment.PostId,
        comment.UserId,
        $"{comment.User.FirstName} {comment.User.LastName}",
        comment.CommentText,
        comment.CreatedAt);
}
