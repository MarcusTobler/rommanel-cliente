using DevPack.Domain.Abstractions.Messaging;
using FluentValidation.Results;

namespace DevPack.Messaging.Commands;

public class CommandResult : ValidationResult, ICommandResult
{
    public bool IsSuccess { get; }
    public bool HasValidationFailure => Errors.Count > 0;
    
    protected CommandResult(bool isSuccess) =>
        (IsSuccess) = (isSuccess);
    protected CommandResult(ValidationResult validationResult) =>
        (IsSuccess, Errors) = (validationResult.Errors.Count > 0, validationResult.Errors);

    public static CommandResult HasSuccess() => new(true);
    public static CommandResult HasFailure(string message) =>
        new(new ValidationResult(new[] { new ValidationFailure("", message) }));
    public static CommandResult HasFailure(string propertyName, string message) => 
        new(new ValidationResult(new[] { new ValidationFailure(propertyName, message) }));
    public static CommandResult HasFailure(ValidationResult validationResult) => 
        new(validationResult);
    public static CommandResult<TValue> HasSuccess<TValue>(TValue value) =>
        new(value, true);
    public static CommandResult<TValue> HasFailure<TValue>(string message) =>
        new(default, new ValidationResult(new[] { new ValidationFailure("", message) }));
    public static CommandResult<TValue> HasFailure<TValue>(string propertyName, string message) =>
        new(default, new ValidationResult(new[] { new ValidationFailure(propertyName, message) }));
    public static CommandResult<TValue> Create<TValue>(TValue? value) =>
        value is not null ? HasSuccess(value) : HasFailure<TValue>("Value is null");
}

public class CommandResult<TValue> : CommandResult
{
    private readonly TValue? _value;

    public CommandResult(TValue? value, bool isSuccess) : base(isSuccess)
    {
        _value = value;
    }

    public CommandResult(TValue? value, ValidationResult validationResult) : base(validationResult)
    {
        _value = value;
    }
    
    public TValue Value => IsSuccess 
        ? _value!
        : throw new InvalidOperationException("The value of a failure result can not be accessed.");
    
    public static implicit operator CommandResult<TValue>(TValue? value) => Create(value);
}