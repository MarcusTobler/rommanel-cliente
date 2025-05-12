using Microsoft.AspNetCore.Mvc;
using Rommanel.WebAPI.Models.Requests;

namespace Rommanel.WebAPI.Routes;

public static class ClienteRoutes
{
    public static RouteGroupBuilder MapClienteRoutes(this IEndpointRouteBuilder builder)
    {
        var clienteGroups = builder.MapGroup("api/v{version:apiVersion}/cliente")
            .HasApiVersion(1.0)
            .ReportApiVersions();

        clienteGroups.MapGet("/", ObterClientesAsync)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .HasApiVersion(1.0);
        
        clienteGroups.MapGet("/{clienteId:Guid}", ObterClienteAsync)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .HasApiVersion(1.0);

        clienteGroups.MapPost("/", RegistrarNovoClienteAsync)
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .HasApiVersion(1.0);
        
        clienteGroups.MapPut("/", AlterarClienteAsync)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .HasApiVersion(1.0);
        
        clienteGroups.MapDelete("/", ExcluirClienteAsync)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .HasApiVersion(1.0);
        
        return clienteGroups;
    }

    private static async Task<IResult> ObterClientesAsync()
    {
        return Results.Ok();
    }
    
    private static async Task<IResult> ObterClienteAsync(Guid clienteId)
    {
        return Results.Ok();
    }
    
    private static async Task<IResult> RegistrarNovoClienteAsync([FromBody] CriarClienteRequest request)
    {
        
        return Results.Created();
    }

    private static async Task<IResult> AlterarClienteAsync([FromBody] CriarClienteRequest request)
    {
        return Results.Ok();
    }
    
    private static async Task<IResult> ExcluirClienteAsync([FromBody] CriarClienteRequest request)
    {
        return Results.Ok();
    }
    
}