using DevPack.Domain.Abstractions;
using DevPack.Domain.Messaging;
using DevPack.Domain.Messaging.Commands;

namespace DevPack.Domain.Mediator;

public interface IMediatorHandler
{
    Task PublishEvent<T>(T @event) where T : Event;
    Task<CommandResult> SendCommand<T>(T command) where T : Command;
    Task<Result<TResponse>> SendQuery<TQuery, TResponse>(TQuery query);
}