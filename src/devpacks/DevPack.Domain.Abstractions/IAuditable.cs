namespace DevPack.Domain.Abstractions;

public interface IAuditable : IEntity
{
    DateTime? CreatedAt { get; }
    string? CreatedBy { get; }
    DateTime? LastUpdatedAt { get; }
    string? LastUpdatedBy { get; }
}