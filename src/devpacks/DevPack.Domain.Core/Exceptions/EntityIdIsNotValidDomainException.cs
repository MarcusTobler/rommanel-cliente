namespace DevPack.Domain.Core.Exceptions;

public sealed class EntityIdIsNotValidDomainException(
    string message) 
    : DomainException(message);