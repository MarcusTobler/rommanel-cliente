// ReSharper disable once CheckNamespace

using DevPack.Data.ReadOnly.Services;

namespace Microsoft.Extensions.DependencyInjection;

public class PostgreSQLDbReadOnlyOptionsExtensions(Action<PostgreSQLDbReadOnlyOptions> options) : IDbReadOnlyOptionsExtensions
{
    public void AddServices(IServiceCollection services)
    {
        services.AddScoped<IDbReadOnlyService, PostgreSQLDbReadOnlyService>();
        services.Configure(options);
    }
}