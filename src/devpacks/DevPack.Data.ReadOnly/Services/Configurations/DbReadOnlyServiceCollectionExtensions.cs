// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class DbReadOnlyServiceCollectionExtensions
{
    public static IServiceCollection AddDbReadOnly(this IServiceCollection services,
        Action<DbReadOnlyOptions> setupAction)
    {
        ArgumentNullException.ThrowIfNull(setupAction, nameof(setupAction));
        
        var options = new DbReadOnlyOptions();
        setupAction(options);
        options.Extensions.ToList().ForEach(serviceExtension =>
        {
            serviceExtension.AddServices(services);
        });

        services.AddSingleton(options);
        
        return services;
    }
}