namespace DevPack.Domain.Core;

public class DomainResult
{
    public bool IsSucceeded { get; }
    public bool IsFailure => !IsSucceeded;
    public Error Error { get; }
    
    protected internal DomainResult(bool isSuccess, Error error) =>
        (IsSucceeded, Error) = (isSuccess, error);
    
    public static DomainResult HasSucceeded() => new(true, Error.None);
    public static DomainResult<TValue> HasSucceeded<TValue>(TValue value) => new(value, true, Error.None);
    
    public static DomainResult HasFailure(Error error) => new(false, error);
    public static DomainResult<TValue> HasFailure<TValue>(Error error) => new(default, false, error);
    public static DomainResult<TValue> ToCreate<TValue>(TValue? value) => value is not null
        ? HasSucceeded(value)
        : HasFailure<TValue>(Error.NullValue);
    
}
