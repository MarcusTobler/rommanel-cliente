using Npgsql;

namespace Microsoft.Extensions.DependencyInjection;

public record PostgreSQLDbReadOnlyOptions
{
    public NpgsqlConnection? Connection { get; set; }
    public NpgsqlDataSource? DataSource { get; set; }
}