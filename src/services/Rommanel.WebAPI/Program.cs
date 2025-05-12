using Asp.Versioning;
using Rommanel.WebAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);
var host = builder.Host;

builder.AddServiceDefaults();

builder.AddApplicationServices();
builder.AddInfrastructureServices();

var withApiVersioning = builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
});
builder.AddOpenApiDefaults(withApiVersioning);

host.AddLoggerConfiguration();

var app = builder.Build();

app.UseApplicationServices();