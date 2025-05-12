using System.Reflection;
using DevPack.Data.EFCore.Outbox;
using DevPack.Domain.Mediator;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Rommanel.Infrastructure.Database.Context;
using Rommanel.WebAPI.Configuration;
using Rommanel.WebAPI.Routes;

namespace Rommanel.WebAPI.Extensions;

public static class ServiceCollectionExtensions
{
    public static IHostApplicationBuilder AddApplicationServices(this IHostApplicationBuilder builder)
    {
        var services = builder.Services;

        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            config.NotificationPublisher = new DomainEventPublisher();
            config.NotificationPublisherType = typeof(DomainEventPublisher);
            config.Lifetime = ServiceLifetime.Transient;
        });

        services.AddTransient<IMediatorHandler, MediatorHandler>();
        
        services.AddDependencyInjectionConfiguration();
        
        return builder;
    }
    
    public static IHostApplicationBuilder AddInfrastructureServices(this IHostApplicationBuilder builder)
    {
        var services = builder.Services;
        var configuration = builder.Configuration;
        
        services.AddHttpContextAccessor();

        services.AddSingleton<InsertOutboxMessagesInterceptor>();
        
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        services.AddDbContext<RommanelDbContext>((serviceProvider, options) =>
        {
            options.UseNpgsql(
                configuration.GetSection("RommanelConfiguration:ClientePostgreSQLConnectionString").Value);
        });
        builder.EnrichNpgsqlDbContext<RommanelDbContext>();
           
        services.AddCors(options =>
        {
            options.AddPolicy("Total", 
                configurePolicy =>
                    configurePolicy
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
        });
        
        return builder;
    }

    public static IApplicationBuilder UseApplicationServices(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        
        app.MapDefaultEndpoints();

        var clienteRoutes = app.NewVersionedApi("clientes");
        clienteRoutes.MapClienteRoutes();    
        
        return app;
    }
    
    public static IApplicationBuilder UserInfrastructureServices(this WebApplication app)
    {
        app.UseCors(configurePolicy =>
        {
            configurePolicy.AllowAnyHeader();
            configurePolicy.AllowAnyMethod();
            configurePolicy.AllowAnyOrigin();
        });

        return app;
    }
}