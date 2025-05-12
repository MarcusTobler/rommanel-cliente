using DevPack.Data.Abstractions;
using DevPack.Domain.Abstractions;

namespace DevPack.Data.EFCore.Abstractions;

public interface IEFRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
{
    
}