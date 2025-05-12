namespace DevPack.Messaging.Abstractions;

public interface IMessage
{
    Guid AggregateId { get; protected set; }
    string MessageType { get; protected set; }
}