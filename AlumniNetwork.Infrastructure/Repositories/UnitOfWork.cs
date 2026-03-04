using AlumniNetwork.Application.Interfaces;
using AlumniNetwork.Infrastructure.DbContext;

namespace AlumniNetwork.Infrastructure.Repositories;

public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => context.SaveChangesAsync(cancellationToken);
}
