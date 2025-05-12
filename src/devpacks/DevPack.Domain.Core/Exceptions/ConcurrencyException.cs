namespace DevPack.Domain.Core.Exceptions;

public sealed class ConcurrencyException(
    string message, 
    Exception innerException)
    : DomainException(message, innerException);