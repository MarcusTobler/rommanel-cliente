using System.Diagnostics.CodeAnalysis;

namespace DevPack.Domain.Abstractions;

public class Result
{
    public bool IsSuccess { get; }

    public Error Error { get; }

    protected Result(bool isSuccess, Error error)
    {
        switch (isSuccess)
        {
            case true when error != Error.None:
                throw new InvalidOperationException();
            case false when error == Error.None:
                throw new InvalidOperationException();
            default:
                IsSuccess = isSuccess;
                Error = error;
                break;
        }
    }
    
    public static Result Success() => new(true, Error.None);

    public static Result Failure(Error error) => new(false, error);

    public static Result<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);

    public static Result<TValue> Failure<TValue>(Error error) => new(default, false, error);

    public static Result<TValue> Create<TValue>(TValue? value) =>
        value is not null ? Success(value) : Failure<TValue>(Error.NullValue);
}

public sealed class Result<TValue> : Result
{
    private readonly TValue? _value;

    public Result(TValue? value, bool isSuccess, Error error)
        : base(isSuccess, error)
    {
        _value = value;
    }

    //[NotNull]
    public TValue? Value => _value; 
//        IsSuccess
//        ? _value
//        : throw new InvalidOperationException("The value of a failure result can not be accessed.");

    public static implicit operator Result<TValue>(TValue? value) => Create(value);
}
