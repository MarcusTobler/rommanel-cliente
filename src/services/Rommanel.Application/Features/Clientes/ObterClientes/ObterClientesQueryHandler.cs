using Dapper;
using DevPack.Data.ReadOnly;
using DevPack.Messaging.Abstractions;

namespace Rommanel.Application.Features.Clientes.ObterClientes;

public class ObterClientesQueryHandler(
    IDbReadOnly dbReadOnly) : IQueryHandler<ObterClientesQuery, IReadOnlyList<ClientesResponse>>
{
    public async Task<Result<IReadOnlyList<ClientesResponse>>> Handle(ObterClientesQuery request, CancellationToken cancellationToken)
    {
        const string clientesSelect =
            """
                SELECT cliente.Id AS Id,
                    pessoafisica.nome AS Nome
                INNER JOIN public.pessoafisica AS pessoafisica
                    ON pessoafisica.id = cliente.id
                ORDER BY cliente.id;
            """;

        var clientes = await dbReadOnly.Connection.QueryAsync(clientesSelect);
        
        return Result.Success<IReadOnlyList<ClientesResponse>>(new List<ClientesResponse>());
    }
}