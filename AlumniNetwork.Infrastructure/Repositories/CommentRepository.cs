using AlumniNetwork.Application.Interfaces;
using AlumniNetwork.Domain.Entities;
using AlumniNetwork.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace AlumniNetwork.Infrastructure.Repositories;

public class CommentRepository(ApplicationDbContext context) : ICommentRepository
{
    public async Task AddAsync(Comment comment, CancellationToken cancellationToken = default)
        => await context.Comments.AddAsync(comment, cancellationToken);

    public async Task<IReadOnlyCollection<Comment>> GetByPostIdAsync(int postId, CancellationToken cancellationToken = default)
        => await context.Comments.AsNoTracking()
            .Include(x => x.User)
            .Where(x => x.PostId == postId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
}
