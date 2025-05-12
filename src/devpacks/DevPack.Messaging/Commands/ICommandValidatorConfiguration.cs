using FluentValidation;

namespace DevPack.Messaging.Commands;

public interface ICommandValidatorConfiguration<TCommand> : IValidator<TCommand> where TCommand : Command
{
    void Validate(CommandValidatorBuilder<TCommand> builder);
}