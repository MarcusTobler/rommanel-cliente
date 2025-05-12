using Npgsql;

namespace Microsoft.Extensions.DependencyInjection;

public static class PostgreSQLDbReadOnlyOptionsBuilderExtensions
{
    public static DbReadOnlyOptions UsePostgreSQL(this DbReadOnlyOptions options, string connectionString)
    {
        return options.UsePostgreSQL(opt =>
        {
            opt.DataSource = NpgsqlDataSource.Create(connectionString);
        });
    }

    public static DbReadOnlyOptions UsePostgreSQL(this DbReadOnlyOptions options, 
        Action<PostgreSQLDbReadOnlyOptions> configure)
    {
        ArgumentNullException.ThrowIfNull(configure, nameof(configure));
        
        options.RegisterExtensions(new PostgreSQLDbReadOnlyOptionsExtensions(configure));

        return options;
    }
}