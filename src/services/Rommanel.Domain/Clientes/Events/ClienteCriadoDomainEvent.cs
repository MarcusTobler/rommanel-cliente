using DevPack.Domain.Messaging;

namespace Rommanel.Domain.Clientes;

public record ClienteCriadoDomainEvent(Guid Id) : DomainEvent;