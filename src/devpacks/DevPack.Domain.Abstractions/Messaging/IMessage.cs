namespace DevPack.Domain.Abstractions.Messaging;

public interface IMessage
{
    Guid AggregateId { get; protected set; }
    string MessageType { get; protected set; }
}