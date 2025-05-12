using FluentValidation;

namespace DevPack.Domain.Messaging.Commands;

public interface ICommandValidator<TCommand> : IValidator<TCommand> where TCommand : Command
{
    void Validate();
}