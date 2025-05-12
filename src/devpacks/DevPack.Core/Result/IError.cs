namespace DevPack.Core.Result;

public interface IError
{
    string Code { get; }
    string Message { get; }
    string PropertyName { get; }
    object? AttemptedValue { get; }
    Exception? Exception { get; }
}