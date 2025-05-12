namespace DevPack.Domain.Abstractions;

public interface IEntity
{
    IReadOnlyCollection<IDomainEvent>? DomainEvents { get; }
    void RaiseDomainEvent(IDomainEvent @event);
    void RemoveDomainEvent(IDomainEvent domainEventBase);
    void ClearDomainEvents();
}
