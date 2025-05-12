namespace DevPack.Data.Abstractions;

public interface ICommandRepository<TEntity> where TEntity : class
{
    IUnitOfWork UnitOfWork { get; }
    
    TEntity Insert(TEntity entity);
    Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default);
    
    ICollection<TEntity> InsertRange(ICollection<TEntity> entities);
    Task<ICollection<TEntity>> InsertRangeAsync(ICollection<TEntity> entities, CancellationToken cancellationToken = default);
    
    void Update(TEntity entity);
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    
    void Delete(TEntity entity);
    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
    
    void Delete(object key);
    Task DeleteAsync(object key, CancellationToken cancellationToken = default);
}