using MediatR;

namespace DevPack.Domain.Abstractions;

public interface IDomainEvent : INotification
{
    DateTime Timestamp { get; protected set; }
}