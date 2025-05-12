using DevPack.Data.ReadOnly;
using DevPack.Messaging.Abstractions;
using DevPack.Messaging.Commands;
using MediatR;
using Rommanel.Application.Features.Clientes.AlterarCliente;
using Rommanel.Application.Features.Clientes.ExcluirCliente;
using Rommanel.Application.Features.Clientes.ObterCliente;
using Rommanel.Application.Features.Clientes.ObterClientes;
using Rommanel.Application.Features.Clientes.RegistrarCliente;

namespace Rommanel.WebAPI.Configuration;

public static class DependencyInjectionConfiguration
{
    public static IServiceCollection AddDependencyInjectionConfiguration(this IServiceCollection services)
    {
        services.AddScoped<IRequestHandler<ObterClientesQuery, Result<IReadOnlyList<ClientesResponse>>>, 
            ObterClientesQueryHandler>();
        services.AddScoped<IRequestHandler<ObterClienteQuery, Result<ClienteResponse>>, 
            ObterClienteQueryHandler>();
        services.AddScoped<IRequestHandler<RegistrarClienteCommand, CommandResult>, 
            RegistrarClienteCommandHandler>();
        services.AddScoped<IRequestHandler<AlterarClienteCommand, CommandResult>, 
            AlterarClienteCommandHandler>();
        services.AddScoped<IRequestHandler<ExcluirClienteCommand, CommandResult>, 
            ExcluirClienteCommandHandler>();
        
        services.AddScoped<IDbReadOnly, DbReadOnly>();
        
        return services;
    }
}