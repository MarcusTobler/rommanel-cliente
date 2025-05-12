namespace DevPack.Messaging.Abstractions;

public record Error(string Code, string Message)
{
    public static readonly Error None = new(string.Empty, string.Empty);

    public static readonly Error NullValue = new("Error.NullValue", "Null value was provided");
    
    public static readonly Error EmptyValue = new("Error.EmptyValue", "Empty value was provided");
    public static readonly Error InvalidValue = new("Error.InvalidValue", "Invalid value was provided");
    
    public static Error NotFound(string message) => new("Error.NotFound", message);
    public static Error Custom(string code, string message) => new(code, message);
    public static Error Custom(string message) => new("Error.Custom", message);
}
