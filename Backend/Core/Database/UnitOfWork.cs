using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Core.Database;

public class UnitOfWork : IUnitOfWork
{
    public DbContext _dbContext;

    public UnitOfWork(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public DbSet<TEntity> GetDbSet<TEntity>() where TEntity : EntityBase
    {
        return _dbContext.Set<TEntity>();
    }

    public virtual Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken) 
    {
        return _dbContext.Database.BeginTransactionAsync(cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}