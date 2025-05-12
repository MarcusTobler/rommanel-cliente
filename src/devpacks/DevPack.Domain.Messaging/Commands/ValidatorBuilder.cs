using FluentValidation;

namespace DevPack.Domain.Messaging.Commands;

public class ValidatorBuilder
{
    private IValidator<Command> _validator;

    public void HasCommandValidator<TCommand>(ICommandValidatorConfiguration<TCommand> configuration)
        where TCommand : Command
    {
        ArgumentNullException.ThrowIfNull(configuration);

        var validator = (AbstractValidator<TCommand>)configuration;
        var builder = new CommandValidatorBuilder<TCommand>(validator);
        configuration.Validate(builder);
        _validator = new ValidatorAdapter<TCommand>(validator); // Usa o adaptador
    }

    public IValidator<Command> Build()
    {
        return _validator ?? new EmptyValidator();
    }

    private class EmptyValidator : AbstractValidator<Command>
    {
        // Validador vazio para casos onde nenhum validador é configurado
    }
}