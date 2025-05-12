using DevPack.Data.ReadOnly;
using DevPack.Messaging.Abstractions;

namespace Rommanel.Application.Features.Clientes.ObterCliente;

public sealed class ObterClienteQueryHandler(
    IDbReadOnly _dbReadOnly) : IQueryHandler<ObterClienteQuery, ClienteResponse>
{
    public async Task<Result<ClienteResponse>> Handle(ObterClienteQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));
        
        return Result.Success(new ClienteResponse());
    }
}