using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace DevPack.Domain.Messaging.Commands;

public abstract record Command : Message, IRequest<CommandResult>
{
    private readonly Lazy<IValidator<Command>> _validator;
    private bool _hasValidationExecuted;
    
    public DateTime Timestamp { get; }
    private ValidationResult ValidationResult { get; set; }

    public IReadOnlyCollection<ValidationFailure> Erros
    {
        get
        {
            if (!_hasValidationExecuted)
            {
                IsValid(); // Força a validação se ainda não foi executada
            }
            return ValidationResult.Errors;
        }
    }
    
    protected Command()
    {
        Timestamp = DateTime.Now;
        ValidationResult = new ValidationResult();
        _hasValidationExecuted = false;
        _validator = new Lazy<IValidator<Command>>(() =>
        {
            var builder = new ValidatorBuilder();
            OnCommandValidating(builder);
            return builder.Build() ?? new EmptyValidator();
        });
    }

    protected virtual void OnCommandValidating(ValidatorBuilder builder) { }

    public bool IsValid()
    {
        ValidationResult = _validator.Value.Validate(this);
        return ValidationResult.IsValid;
    }

    protected async Task<bool> IsValidAsync(CancellationToken? cancellationToken = default)
    {
        ValidationResult = await _validator.Value.ValidateAsync(this, cancellationToken ?? CancellationToken.None);
        return ValidationResult.IsValid;
    }
    
    private class EmptyValidator : AbstractValidator<Command>
    {
        // Validador vazio para casos onde nenhum validador é configurado
    }
}

public abstract record Command<TResponse> : Command, IRequest<CommandResult<TResponse>> { }
