using System.Linq.Expressions;
using DevPack.Core.Helpers.Pagination;

namespace DevPack.Data.Abstractions;

public interface IQueryRepository<TEntity> where TEntity : class
{
    IQueryable<TEntity> GetAll(bool? asNoTracking = true);
    IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] includeProperties);
    PaginationResponse<TEntity> GetAllPagination(PagedListParameters parameters);

    TEntity? Find(params object[] keyValues);
    Task<TEntity?> FindAsync(object[] keyValues, CancellationToken cancellationToken = default);
    TEntity? FindOne(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity?> FindOneAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    IQueryable<TEntity> FindWhere(Expression<Func<TEntity, bool>> predicate);
    Task<ICollection<TEntity>> FindWhereAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    PaginationResponse<TEntity> FindWherePagination(Expression<Func<TEntity, bool>> predicate, PagedListParameters parameters);
    Task<PaginationResponse<TEntity>> FindWherePaginationAsync(Expression<Func<TEntity, bool>> predicate, PagedListParameters parameters, CancellationToken cancellationToken = default);

    int Count();
    Task<int> CountAsync(CancellationToken cancellationToken = default);
    int Count(Expression<Func<TEntity, bool>> predicate);
    Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    bool Any();
    Task<bool> AnyAsync(CancellationToken cancellationToken = default);
    bool Any(Expression<Func<TEntity, bool>> predicate);
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    (bool IsSuccess, bool IsExist, TEntity? Entity) HasExists(params object[] keyValues);
    Task<(bool IsSuccess, bool IsExist, TEntity? Entity)> HasExistsAsync(object[] keyValues, CancellationToken cancellationToken = default);
    (bool IsSuccess, bool IsExist, TEntity? Entity) HasExists(Expression<Func<TEntity, bool>> predicate);
    Task<(bool IsSuccess, bool IsExist, TEntity? Entity)> HasExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
}