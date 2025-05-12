using DevPack.Data.Abstractions;
using DevPack.Domain.Abstractions;
using Microsoft.EntityFrameworkCore.Storage;

namespace DevPack.Data.EFCore.Abstractions;

public interface IEFUnitOfWork : IUnitOfWork
{
    IEFRepository<TEntity>? Repository<TEntity>() where TEntity : class, IEntity, IAggregateRoot;
    bool HasActiveTransaction { get; }
    Task<IDbContextTransaction> BeginTransactionAsync();
}