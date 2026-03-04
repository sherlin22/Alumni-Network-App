using AlumniNetwork.Application.Interfaces;
using AlumniNetwork.Domain.Entities;
using AlumniNetwork.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace AlumniNetwork.Infrastructure.Repositories;

public class LikeRepository(ApplicationDbContext context) : ILikeRepository
{
    public async Task<Like?> GetAsync(int postId, int userId, CancellationToken cancellationToken = default)
        => await context.Likes.FirstOrDefaultAsync(x => x.PostId == postId && x.UserId == userId, cancellationToken);

    public async Task<int> CountByPostIdAsync(int postId, CancellationToken cancellationToken = default)
        => await context.Likes.CountAsync(x => x.PostId == postId, cancellationToken);

    public async Task AddAsync(Like like, CancellationToken cancellationToken = default)
        => await context.Likes.AddAsync(like, cancellationToken);
}
