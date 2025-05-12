using FluentValidation;

namespace DevPack.Messaging.Commands;

public interface ICommandValidator<TCommand> : IValidator<TCommand> where TCommand : Command
{
    void Validate();
}