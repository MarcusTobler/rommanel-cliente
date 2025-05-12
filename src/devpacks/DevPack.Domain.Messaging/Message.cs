namespace DevPack.Domain.Messaging;

public abstract record Message
{
    public Guid AggregateId { get; protected set; }
    public string MessageType { get; protected set; }

    protected Message() => MessageType = GetType().Name;
}