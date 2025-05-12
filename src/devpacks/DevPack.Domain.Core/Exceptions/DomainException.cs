using DevPack.Core.Exceptions;

namespace DevPack.Domain.Core.Exceptions;

public class DomainException : DevPackException
{
    protected DomainException(string message) : base(message) { }
    protected DomainException(string message, Exception innerException) : base(message, innerException) { }
}