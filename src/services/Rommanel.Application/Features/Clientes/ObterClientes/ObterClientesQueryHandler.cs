using DevPack.Data.ReadOnly;
using DevPack.Messaging.Abstractions;

namespace Rommanel.Application.Features.Clientes.ObterClientes;

public class ObterClientesQueryHandler(
    IDbReadOnly _dbReadOnly) : IQueryHandler<ObterClientesQuery, IReadOnlyList<ClientesResponse>>
{
    public async Task<Result<IReadOnlyList<ClientesResponse>>> Handle(ObterClientesQuery request, CancellationToken cancellationToken)
    {
        
        return Result.Success<IReadOnlyList<ClientesResponse>>(new List<ClientesResponse>());
    }
}