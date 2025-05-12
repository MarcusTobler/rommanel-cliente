using System.Data;
using DevPack.Data.ReadOnly.Services;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Microsoft.Extensions.DependencyInjection;

public class PostgreSQLDbReadOnlyService(PostgreSQLDbReadOnlyOptions options) : IDbReadOnlyService
{
    public IDbConnection? Connection { get; }

    public PostgreSQLDbReadOnlyService(IOptions<PostgreSQLDbReadOnlyOptions> options) : this(options.Value)
    {
        Connection = options.Value.DataSource?.CreateConnection();
        Connection?.Open();

        var teste = Connection?.ConnectionString;
    }
    
}