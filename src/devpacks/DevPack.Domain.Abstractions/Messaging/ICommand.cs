using System.ComponentModel.DataAnnotations;
using MediatR;

namespace DevPack.Domain.Abstractions.Messaging;

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