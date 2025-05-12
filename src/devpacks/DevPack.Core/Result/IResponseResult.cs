namespace DevPack.Core.Result;

public interface IResponseResult
{
    bool IsSucceeded { get; }
    bool IsNotFound { get; }
    Error? ErrorInfo { get; }
    bool HasError { get; }
    object? Result { get; }
}