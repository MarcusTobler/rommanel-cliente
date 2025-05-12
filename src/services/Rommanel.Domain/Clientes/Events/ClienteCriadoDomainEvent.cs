using DevPack.Messaging.Domain;

namespace Rommanel.Domain.Clientes.Events;

public record ClienteCriadoDomainEvent(Guid Id) : DomainEvent;