namespace DevPack.Domain.Core;

public class Error : IEquatable<Error>
{
    public static readonly Error None = new(string.Empty, string.Empty);
    public static readonly Error NullValue = new("Error.NullValue", "The specified result value is null");
    
    public string Code { get; }
    public string Message { get; }
    
    protected Error(string code, string message) =>
        (Code, Message) = (code, message);
    
    public static Error HasError(string code, string message) => new(code, message);

    public static implicit operator string(Error error) => error.Code;

    public static bool operator ==(Error? a, Error? b)
    {
        if (a is null && b is null) return true;
        if (a is null || b is null) return false;

        return a.Equals(b);
    }
    
    public static bool operator !=(Error? a, Error? b) => !(a == b);
    
    public virtual bool Equals(Error? other)
    {
        if (ReferenceEquals(null, other)) return false;
        
        if (ReferenceEquals(this, other)) return true;
        
        return Code == other.Code && Message == other.Message;
    }
    
    public override bool Equals(object? obj) => obj is Error error && Equals(error);
    
    public override int GetHashCode() => HashCode.Combine(Code, Message);
    
    public override string ToString() => $"{Code}: {Message}";
}