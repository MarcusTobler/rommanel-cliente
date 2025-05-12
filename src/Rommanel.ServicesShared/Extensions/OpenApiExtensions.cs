using Asp.Versioning;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Rommanel.ServicesShared.Configuration;
using Rommanel.ServicesShared.Extensions;
using Swashbuckle.AspNetCore.SwaggerGen;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.Hosting;

public static partial class Extensions
{
    public static IHostApplicationBuilder AddOpenApiDefaults(this IHostApplicationBuilder builder,
        IApiVersioningBuilder? apiVersioning = default)
    {
        var services = builder.Services;
        var configuration = builder.Configuration;
        var openApi = configuration.GetSection("OpenApi");

        if (!openApi.Exists() || apiVersioning is null)
            return builder;

        services.AddEndpointsApiExplorer();
        
        apiVersioning.AddApiExplorer(options => options.GroupNameFormat = "'v'VVV");
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        
        services.AddSwaggerGen(options => options.OperationFilter<OpenApiDefaultValues>());

        return builder;
    }
    
    public static IApplicationBuilder UseDefaultOpenApi(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(setup =>
        {
            app.DescribeApiVersions().ToList().ForEach(x => 
                setup.SwaggerEndpoint($"{x.GroupName}/swagger.json", x.GroupName));
        });
            
        app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();
        
        return app;
    }
}