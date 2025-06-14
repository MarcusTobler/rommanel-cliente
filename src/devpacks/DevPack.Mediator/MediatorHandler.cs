using System.Runtime.CompilerServices;
using DevPack.Messaging.Abstractions;
using DevPack.Messaging.Commands;
using MediatR;

namespace DevPack.Domain.Mediator;

public class MediatorHandler : IMediatorHandler
{
    private readonly IMediator _mediator;
    
    public MediatorHandler(IMediator mediator) => _mediator = mediator;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public async Task<CommandResult> SendCommand<TCommand>(TCommand command) where TCommand : Command =>
        await _mediator.Send(command);

    public async Task<Result<TResponse>> SendQuery<TQuery, TResponse>(TQuery query) =>
        (Result<TResponse>)await _mediator.Send(query);
        
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public async Task PublishEvent<T>(T @event) => 
        await _mediator.Publish(@event);

}