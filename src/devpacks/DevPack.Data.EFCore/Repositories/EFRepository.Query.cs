using System.Linq.Expressions;
using DevPack.Core.Helpers.Pagination;
using DevPack.Data.EFCore.Abstractions;
using DevPack.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace DevPack.Data.EFCore.Repositories;

public abstract partial class EFRepository<TContext, TEntity> : IEFRepository<TEntity>
    where TContext : DbContext 
    where TEntity : class, IEntity
{
    public virtual IQueryable<TEntity> GetAll(bool? asNoTracking = true)
    {
        return asNoTracking ?? true
            ? Context.Set<TEntity>().AsNoTracking()
            : Context.Set<TEntity>().AsQueryable();
    }

    public virtual IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] includeProperties)
    {
        return includeProperties.Aggregate(GetAll(), 
            (current, includeProperty) => current.Include(includeProperty));
    }

    public virtual PaginationResponse<TEntity> GetAllPagination(PagedListParameters parameters)
    {
        return PagedList<TEntity>.HasPagination(GetAll(), parameters.CurrentPage, parameters.PageSize);
    }

    public virtual TEntity? Find(params object[] keyValues)
    {
        return Context.Set<TEntity>().Find(keyValues);
    }

    public virtual async Task<TEntity?> FindAsync(object[] keyValues, CancellationToken cancellationToken = default)
    {
        return await Context.Set<TEntity>().FindAsync(keyValues, cancellationToken);
    }

    public virtual TEntity? FindOne(Expression<Func<TEntity, bool>> predicate)
    {
        return Context.Set<TEntity>().First(predicate);
    }

    public virtual async Task<TEntity?> FindOneAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await Context.Set<TEntity>().FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public virtual IQueryable<TEntity> FindWhere(Expression<Func<TEntity, bool>> predicate)
    {
        return Context.Set<TEntity>().Where(predicate);
    }

    public virtual async Task<ICollection<TEntity>> FindWhereAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await Context.Set<TEntity>().Where(predicate).ToListAsync(cancellationToken);
    }

    public virtual PaginationResponse<TEntity> FindWherePagination(Expression<Func<TEntity, bool>> predicate, PagedListParameters parameters)
    {
        return PagedList<TEntity>.HasPagination(Context.Set<TEntity>().Where(predicate), parameters.CurrentPage, parameters.PageSize);
    }
    
    public virtual async Task<PaginationResponse<TEntity>> FindWherePaginationAsync(Expression<Func<TEntity, bool>> predicate, PagedListParameters parameters, CancellationToken cancellationToken = default)
    {
        return PagedList<TEntity>.HasPagination(
            await Context.Set<TEntity>().Where(predicate).ToListAsync(cancellationToken), parameters.CurrentPage,
            parameters.PageSize);
    }

    public int Count()
    {
        return Context.Set<TEntity>().Count();
    }

    public virtual async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return await Context.Set<TEntity>().CountAsync(cancellationToken);
    }

    public virtual int Count(Expression<Func<TEntity, bool>> predicate)
    {
        return Context.Set<TEntity>().Where(predicate).Count();
    }

    public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await Context.Set<TEntity>().Where(predicate).CountAsync(cancellationToken);
    }

    public virtual bool Any()
    {
        return Context.Set<TEntity>().Any();
    }

    public virtual async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
    {
        return await Context.Set<TEntity>().AnyAsync(cancellationToken);
    }

    public virtual bool Any(Expression<Func<TEntity, bool>> predicate)
    {
        return Context.Set<TEntity>().Where(predicate).Any();
    }

    public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await Context.Set<TEntity>().Where(predicate).AnyAsync(cancellationToken);
    }

    public (bool IsSuccess, bool IsExist, TEntity? Entity) HasExists(params object[] keyValues)
    {
        try
        {
            var entity = Context.Set<TEntity>().Find(keyValues);
            return (true, entity is not null, entity);
        }
        catch (Exception)
        {
            return (false, false, null);
        }
    }

    public virtual async Task<(bool IsSuccess, bool IsExist, TEntity? Entity)> HasExistsAsync(object[] keyValues, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = await Context.Set<TEntity>().FindAsync(keyValues, cancellationToken);
            return (true, entity is not null, entity);
        }
        catch (Exception)
        {
            return (false, false, null);
        }
    }

    public virtual (bool IsSuccess, bool IsExist, TEntity? Entity) HasExists(Expression<Func<TEntity, bool>> predicate)
    {
        try
        {
            var entity = Context.Set<TEntity>().FirstOrDefault(predicate);
            return (true, entity is not null, entity);
        }
        catch (Exception)
        {
            return (false, false, null);
        }
    }

    public virtual async Task<(bool IsSuccess, bool IsExist, TEntity? Entity)> HasExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = await Context.Set<TEntity>().FirstOrDefaultAsync(predicate, cancellationToken);
            return (true, entity is not null, entity);
        }
        catch (Exception)
        {
            return (false, false, null);
        }
    }
}