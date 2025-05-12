using DevPack.Messaging.Abstractions;
using DevPack.Messaging.Commands;

namespace DevPack.Domain.Mediator;

public interface IMediatorHandler
{
    Task PublishEvent<T>(T @event);
    Task<CommandResult> SendCommand<T>(T command) where T : Command;
    Task<Result<TResponse>> SendQuery<TQuery, TResponse>(TQuery query);
}