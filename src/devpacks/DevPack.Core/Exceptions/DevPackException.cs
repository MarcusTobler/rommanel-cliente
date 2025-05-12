namespace DevPack.Core.Exceptions;

public class DevPackException : Exception
{
    public DevPackException() { }
    public DevPackException(string message) : base(message) { }
    public DevPackException(string message, Exception innerException) : base(message, innerException) { }
}