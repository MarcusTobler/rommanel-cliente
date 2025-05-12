using DevPack.Domain.Abstractions.Messaging;
using MediatR;

namespace DevPack.Messaging.Abstractions;

public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, ICommandResult>
    where TCommand: ICommand
{
    
}

public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, ICommandResult<TResponse>>
    where TCommand : ICommand<TResponse>
{
    
}