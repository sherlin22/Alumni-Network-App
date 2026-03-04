namespace AlumniNetwork.Application.DTOs;

public record CreatePostRequestDto(string Content, string? ImageUrl);

public record PostResponseDto(
    int Id,
    int UserId,
    string AuthorName,
    string Department,
    int YearOfJoining,
    string Content,
    string? ImageUrl,
    DateTime CreatedAt,
    int LikeCount,
    int CommentCount);

public record PostQueryDto(string? Department, int? Year, string? Location, int PageNumber = 1, int PageSize = 10);

public record AddCommentRequestDto(string CommentText);

public record CommentResponseDto(int Id, int PostId, int UserId, string AuthorName, string CommentText, DateTime CreatedAt);
