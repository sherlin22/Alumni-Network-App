using AlumniNetwork.Application.DTOs;
using AlumniNetwork.Domain.Entities;
using AlumniNetwork.Domain.ValueObjects;

namespace AlumniNetwork.Application.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<PagedResult<User>> SearchAsync(AlumniSearchQueryDto query, CancellationToken cancellationToken = default);
    Task<PagedResult<User>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task AddAsync(User user, CancellationToken cancellationToken = default);
    void Update(User user);
    void Delete(User user);
}

public interface IPostRepository
{
    Task<Post?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<PagedResult<Post>> GetPagedAsync(PostQueryDto query, CancellationToken cancellationToken = default);
    Task AddAsync(Post post, CancellationToken cancellationToken = default);
    void Delete(Post post);
}

public interface ICommentRepository
{
    Task AddAsync(Comment comment, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Comment>> GetByPostIdAsync(int postId, CancellationToken cancellationToken = default);
}

public interface ILikeRepository
{
    Task<Like?> GetAsync(int postId, int userId, CancellationToken cancellationToken = default);
    Task<int> CountByPostIdAsync(int postId, CancellationToken cancellationToken = default);
    Task AddAsync(Like like, CancellationToken cancellationToken = default);
}

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
