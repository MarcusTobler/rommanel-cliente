using System.Collections;
using DevPack.Data.EFCore.Abstractions;
using DevPack.Domain.Abstractions;
using DevPack.Domain.Core.Exceptions;
using DevPack.Domain.Messaging;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace DevPack.Data.EFCore.Contexts;

public abstract class EFDbContext<TContext> : DbContext, IEFUnitOfWork where TContext : DbContext
{
    private readonly DbContextOptions<TContext> _contextOptions;

    protected EFDbContext(DbContextOptions<TContext> contextOptions) 
        : base(contextOptions) =>
        (_contextOptions) = (contextOptions);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Ignore<ValidationResult>();
        modelBuilder.Ignore<Event>();
        
        base.OnModelCreating(modelBuilder);
    }

    #region Repository
    private Hashtable? _repositories;
    
    public IEFRepository<TEntity> Repository<TEntity>() where TEntity : class, IEntity, IAggregateRoot
    {
        _repositories ??= new Hashtable();
        
        var type = typeof(TEntity).Name;

        if (_repositories.ContainsKey(type)) 
            return (IEFRepository<TEntity>)_repositories[type];
        
        var repositoryType = typeof(EFDbContext<>);
        var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), this);

        _repositories.Add(type, repositoryInstance);

        return (IEFRepository<TEntity>)_repositories[type];
    }
    #endregion
    
    #region UnitOfWork
    private IDbContextTransaction? _currentTransaction;
    
    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return _currentTransaction ?? (_currentTransaction = await Database.BeginTransactionAsync());
    }
    
    public IDbContextTransaction CurrentTransaction() => _currentTransaction;
    public bool HasActiveTransaction => _currentTransaction is not null;
    
    public bool Commit()
    {
        return base.SaveChanges() > 0;
    }

    public async Task<bool> CommitAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await base.SaveChangesAsync(cancellationToken) > 0;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new ConcurrencyException("Concurrency exception occurred.", ex);
        }
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new ConcurrencyException("Concurrency exception occurred.", ex);
        }
    }
    #endregion
    
}