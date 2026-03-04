using AlumniNetwork.Application.DTOs;
using AlumniNetwork.Application.Interfaces;
using AlumniNetwork.Domain.Entities;
using AlumniNetwork.Domain.ValueObjects;
using AlumniNetwork.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace AlumniNetwork.Infrastructure.Repositories;

public class UserRepository(ApplicationDbContext context) : IUserRepository
{
    public async Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await context.Users.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        => await context.Users.FirstOrDefaultAsync(x => x.Email == email.ToLower(), cancellationToken);

    public async Task<PagedResult<User>> SearchAsync(AlumniSearchQueryDto query, CancellationToken cancellationToken = default)
    {
        var usersQuery = context.Users.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Department))
        {
            usersQuery = usersQuery.Where(x => x.Department == query.Department);
        }

        if (query.Year.HasValue)
        {
            usersQuery = usersQuery.Where(x => x.YearOfJoining == query.Year.Value);
        }

        if (!string.IsNullOrWhiteSpace(query.Location))
        {
            usersQuery = usersQuery.Where(x => x.CurrentLocation == query.Location);
        }

        var totalCount = await usersQuery.CountAsync(cancellationToken);
        var data = await usersQuery
            .OrderByDescending(x => x.CreatedAt)
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<User>
        {
            TotalCount = totalCount,
            PageNumber = query.PageNumber,
            PageSize = query.PageSize,
            Data = data
        };
    }

    public async Task<PagedResult<User>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var totalCount = await context.Users.CountAsync(cancellationToken);
        var users = await context.Users.AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<User>
        {
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize,
            Data = users
        };
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
        => await context.Users.AddAsync(user, cancellationToken);

    public void Update(User user) => context.Users.Update(user);

    public void Delete(User user) => context.Users.Remove(user);
}
