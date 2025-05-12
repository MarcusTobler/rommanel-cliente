using DevPack.Messaging.Abstractions;

namespace Rommanel.Application.Features.Clientes.ObterCliente;

public sealed record ObterClienteQuery(Guid ClienteId) : IQuery<ClienteResponse>;