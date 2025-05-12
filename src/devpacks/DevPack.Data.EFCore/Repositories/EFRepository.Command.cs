using DevPack.Data.EFCore.Abstractions;
using DevPack.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace DevPack.Data.EFCore.Repositories;

public abstract partial class EFRepository<TContext, TEntity> : IEFRepository<TEntity>
    where TContext : DbContext 
    where TEntity : class, IEntity
{
    public virtual TEntity Insert(TEntity entity)
    {
        var result = Context.Set<TEntity>().Add(entity);
        return result.Entity;
    }

    public virtual async Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var result = await Context.Set<TEntity>().AddAsync(entity, cancellationToken);
        return result.Entity;
    }
    
    public virtual ICollection<TEntity> InsertRange(ICollection<TEntity> entities)
    {
        Context.Set<TEntity>().AddRange(entities);
        return entities;
    }
    
    public virtual async Task<ICollection<TEntity>> InsertRangeAsync(ICollection<TEntity> entities, CancellationToken cancellationToken = default)
    {
        await Context.Set<TEntity>().AddRangeAsync(entities, cancellationToken);
        return entities;
    }

    public virtual void Update(TEntity entity)
    {
        if (Context.Entry(entity).State == EntityState.Detached)
            Context.Set<TEntity>().Attach(entity);

        Context.Entry(entity).State = EntityState.Modified;
    }

    public virtual async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        Update(entity);
        await Task.CompletedTask;
    }

    public virtual void Delete(TEntity entity)
    {
        if (Context.Entry(entity).State == EntityState.Detached)
            Context.Set<TEntity>().Attach(entity);

        Context.Set<TEntity>().Remove(entity);
    }

    public virtual async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        Delete(entity);
        await Task.CompletedTask;
    }

    public virtual void Delete(object key)
    {
        var item = Context.Set<TEntity>().Find(key);
        if (item is not null)
            Context.Set<TEntity>().Remove(item);
    }

    public virtual async Task DeleteAsync(object key, CancellationToken cancellationToken = default)
    {
        var item = await Context.Set<TEntity>().FindAsync(new[] { key, cancellationToken }, cancellationToken: cancellationToken);
        if (item is not null)
            Context.Set<TEntity>().Remove(item);
    }
}