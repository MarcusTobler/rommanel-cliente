using DevPack.Messaging.Commands;

namespace Rommanel.Application.Features.Clientes.ExcluirCliente;

public sealed record ExcluirClienteCommand(Guid ClienteId) : Command;