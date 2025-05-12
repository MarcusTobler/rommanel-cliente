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

        clienteGroups.MapPost("/", RegistrarNovoClienteAsync)
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError);
        
        return clienteGroups;
    }

    private static async Task<IResult> RegistrarNovoClienteAsync(
        [FromServices] ClienteRouteService services,
        [FromBody] CriarClienteRequest request)
    {
        
        return Results.Created();
    }
}