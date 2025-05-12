using DevPack.Domain.Abstractions;

namespace DevPack.Data.Abstractions;

public interface IRepository<TEntity> : ICommandRepository<TEntity>, IQueryRepository<TEntity>, IDisposable 
    where TEntity : class, IEntity { }