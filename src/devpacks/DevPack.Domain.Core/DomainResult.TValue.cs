using System.Diagnostics.CodeAnalysis;

namespace DevPack.Domain.Core;

public class DomainResult<TValue> : DomainResult
{
    private readonly TValue? _value;
    
    protected internal DomainResult(TValue? value, bool isSuccess, Error error) : base(isSuccess, error) =>
        _value = value;
    
    [NotNull]
    public TValue Value => IsSucceeded
        ? _value!
        : throw new InvalidOperationException("The value of a failure result can not be accessed.");
    
    public static implicit operator DomainResult<TValue>(TValue value) => ToCreate(value);
    
    public static DomainResult<TValue> RuleFor(Func<TValue, bool> predicate, string errorMessage) =>
        !predicate(default!)
            ? HasFailure<TValue>(Error.HasError("RuleViolation", errorMessage))
            : default!;
}