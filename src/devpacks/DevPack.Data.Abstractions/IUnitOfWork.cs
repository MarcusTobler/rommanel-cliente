namespace DevPack.Data.Abstractions;

public interface IUnitOfWork
{
    bool Commit();
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task<bool> CommitAsync(CancellationToken cancellationToken = default);
}