using DevPack.Data.Abstractions;
using DevPack.Data.EFCore.Abstractions;
using DevPack.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace DevPack.Data.EFCore.Repositories;

public abstract partial class EFRepository<TContext, TEntity> : IEFRepository<TEntity>
    where TContext : DbContext 
    where TEntity : class, IEntity
{
    protected readonly TContext Context;
    
    protected IQueryable<TEntity> Entity => Context.Set<TEntity>();

    public abstract IUnitOfWork UnitOfWork { get; } 

    public EFRepository(TContext dbContext) => 
        (Context) = (dbContext);
    
    public void Dispose()
    {
        Context?.Dispose();
        GC.SuppressFinalize(this);
    }
}