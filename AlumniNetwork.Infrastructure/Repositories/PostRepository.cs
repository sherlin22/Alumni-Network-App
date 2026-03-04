using AlumniNetwork.Application.DTOs;
using AlumniNetwork.Application.Interfaces;
using AlumniNetwork.Domain.Entities;
using AlumniNetwork.Domain.ValueObjects;
using AlumniNetwork.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace AlumniNetwork.Infrastructure.Repositories;

public class PostRepository(ApplicationDbContext context) : IPostRepository
{
    public async Task<Post?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await context.Posts
            .Include(x => x.User)
            .Include(x => x.Comments)
            .Include(x => x.Likes)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<PagedResult<Post>> GetPagedAsync(PostQueryDto query, CancellationToken cancellationToken = default)
    {
        var postsQuery = context.Posts.AsNoTracking()
            .Include(x => x.User)
            .Include(x => x.Comments)
            .Include(x => x.Likes)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Department))
        {
            postsQuery = postsQuery.Where(x => x.User.Department == query.Department);
        }

        if (query.Year.HasValue)
        {
            postsQuery = postsQuery.Where(x => x.User.YearOfJoining == query.Year.Value);
        }

        if (!string.IsNullOrWhiteSpace(query.Location))
        {
            postsQuery = postsQuery.Where(x => x.User.CurrentLocation == query.Location);
        }

        var totalCount = await postsQuery.CountAsync(cancellationToken);
        var posts = await postsQuery
            .OrderByDescending(x => x.CreatedAt)
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<Post>
        {
            TotalCount = totalCount,
            PageNumber = query.PageNumber,
            PageSize = query.PageSize,
            Data = posts
        };
    }

    public async Task AddAsync(Post post, CancellationToken cancellationToken = default)
        => await context.Posts.AddAsync(post, cancellationToken);

    public void Delete(Post post) => context.Posts.Remove(post);
}
