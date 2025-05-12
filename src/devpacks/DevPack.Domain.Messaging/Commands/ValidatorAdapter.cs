using FluentValidation;
using FluentValidation.Results;

namespace DevPack.Domain.Messaging.Commands;

public class ValidatorAdapter<TCommand> : IValidator<Command> where TCommand : Command
{
    private readonly IValidator<TCommand> _validator;

    public ValidatorAdapter(IValidator<TCommand> validator)
    {
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
    }

    public ValidationResult Validate(IValidationContext context)
    {
        if (context.InstanceToValidate is TCommand command)
        {
            return _validator.Validate(context);
        }
        return new ValidationResult(new[] { new ValidationFailure("Command", $"Expected type {typeof(TCommand).Name}, but got {context.InstanceToValidate?.GetType().Name}") });
    }

    public ValidationResult Validate(Command instance)
    {
        if (instance is TCommand command)
        {
            return _validator.Validate(command);
        }
        return new ValidationResult(new[] { new ValidationFailure("Command", $"Expected type {typeof(TCommand).Name}, but got {instance?.GetType().Name}") });
    }

    public Task<ValidationResult> ValidateAsync(IValidationContext context, CancellationToken cancellation = default)
    {
        if (context.InstanceToValidate is TCommand command)
        {
            return _validator.ValidateAsync(context, cancellation);
        }
        return Task.FromResult(new ValidationResult(new[] { new ValidationFailure("Command", $"Expected type {typeof(TCommand).Name}, but got {context.InstanceToValidate?.GetType().Name}") }));
    }

    public Task<ValidationResult> ValidateAsync(Command instance, CancellationToken cancellation = default)
    {
        if (instance is TCommand command)
        {
            return _validator.ValidateAsync(command, cancellation);
        }
        return Task.FromResult(new ValidationResult(new[] { new ValidationFailure("Command", $"Expected type {typeof(TCommand).Name}, but got {instance?.GetType().Name}") }));
    }

    public IValidatorDescriptor CreateDescriptor() => _validator.CreateDescriptor();

    public bool CanValidateInstancesOfType(Type type) => typeof(TCommand).IsAssignableFrom(type);
}