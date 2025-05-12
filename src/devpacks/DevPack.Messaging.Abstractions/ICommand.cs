using System.ComponentModel.DataAnnotations;
using DevPack.Domain.Abstractions.Messaging;
using MediatR;

namespace DevPack.Messaging.Abstractions;

public interface ICommand : IRequest<ICommandResult>, IBaseCommand
{
}

public interface ICommand<TResponse> : IRequest<ICommandResult<TResponse>>, IBaseCommand
{
    
}

public interface IBaseCommand
{
    DateTime Timestamp { get; }
    ValidationResult ValidationResult { get; }
    
}