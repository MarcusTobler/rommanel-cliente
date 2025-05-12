using DevPack.Domain.Abstractions;

namespace DevPack.Messaging.Domain;

public record Event : Message, IDomainEvent
{
    public DateTime Timestamp { get; set; }
    
    protected Event() => Timestamp = DateTime.Now;
}