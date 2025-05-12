using MediatR;

namespace DevPack.Domain.Abstractions.Messaging;

public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, ICommandResult>
    where TCommand: ICommand
{
    
}

public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, ICommandResult<TResponse>>
    where TCommand : ICommand<TResponse>
{
    
}