using DevPack.Messaging.Abstractions;

namespace Rommanel.Application.Features.Clientes.ObterClientes;

public sealed record ObterClientesQuery(): IQuery<IReadOnlyList<ClientesResponse>>;