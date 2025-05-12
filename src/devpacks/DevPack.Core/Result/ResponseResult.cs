namespace DevPack.Core.Result;

public sealed record ResponseResult : IResponseResult
{
    public bool IsSucceeded { get; }
    public bool IsNotFound { get; }
    public Error? ErrorInfo { get; }
    
    public bool HasError => ErrorInfo != null;
    
    public object? Result { get; }

    private ResponseResult() =>
        (IsSucceeded, IsNotFound, Result) = (false, false, null);
    private ResponseResult(bool isSucceeded, object? result = null) =>
        (IsSucceeded, IsNotFound, Result) = (isSucceeded, false, result);
    private ResponseResult(string codeError, string messageError) =>
        (IsSucceeded, IsNotFound, ErrorInfo, Result) = (false, false, Error.FromError(codeError, messageError), null);
    private ResponseResult(string messageError, Exception? exception = null) =>
        (IsSucceeded, IsNotFound, ErrorInfo, Result) = (false, false, Error.FromError(messageError, exception), null);
    private ResponseResult(Exception exception) =>
        (IsSucceeded, IsNotFound, ErrorInfo, Result) = (false, false, Error.FromException(exception), null);
    private ResponseResult(Error? error) =>
        (IsSucceeded, IsNotFound, ErrorInfo, Result) = (false, false, error, null);
    private ResponseResult(bool isNotFound) =>
        (IsSucceeded, IsNotFound, ErrorInfo, Result) = (false, isNotFound, null, null);

    public static ResponseResult HasSuccess(object? result = null) => new(true, result);
    
    public static ResponseResult HasFailure() => new();
    public static ResponseResult HasFailure(object? result) => new(false, result);
    public static ResponseResult HasFailure(string codeError, string messageError) => new(codeError, messageError);
    public static ResponseResult HasFailure(string messageError) => new(messageError);
    public static ResponseResult HasFailure(Exception exception) => new(exception);
    public static ResponseResult HasFailure(Error? error) => new(error);

    public static ResponseResult HasNotFound() => new(true);
}