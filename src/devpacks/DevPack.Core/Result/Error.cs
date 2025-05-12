namespace DevPack.Core.Result;

public record Error : IError
{
    public string Code { get; protected set; } = "ER0000";
    public string Message { get; protected set; }
    public string? PropertyName { get; protected set; }
    public object? AttemptedValue { get; protected set; }
    public Exception? Exception { get; protected set; }
    
    protected Error() => (Code, Message) = ("ER0000", "Error has occurred.");
    protected Error(Exception exception) : this(exception.Message, exception) { }
    protected Error(string message, Exception? exception) => 
        (Message, Exception) = (message, exception);
    protected Error(string code, string message, Exception? exception) => 
        (Code, Message, Exception) = (code, message, exception);
    protected Error(string code, string message, string propertyName, object? attemptedValue, Exception? exception) =>
        (Code, Message, PropertyName, AttemptedValue, Exception) = (code, message, propertyName, attemptedValue,exception);

    public static Error Empty => new();
    public static Error FromException(Exception exception) => 
        new(exception);
    public static Error FromError(string message, Exception? exception = null) => 
        new(message, exception);
    public static Error FromError(string code, string message, Exception? exception = null) =>
        new(code, message, exception);
    public static Error FromError(string code, string message, string propertyName, object? attemptedValue = null,
        Exception? exception = null) =>
        new(code, message, propertyName, attemptedValue, exception);
}