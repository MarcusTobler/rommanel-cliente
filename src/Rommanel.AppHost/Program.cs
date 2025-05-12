var builder = DistributedApplication.CreateBuilder(args);

var username = builder.AddParameter(name: "username", value: "rommanel", secret: true);
var password = builder.AddParameter(name: "password", value: "rommanel", secret: true);

const string databaseName = "rommaneldb";
const string creationScript = $"CREATE DATABASE {databaseName};";
var postgres = builder.AddPostgres(name: "postgres", userName: username, password: password);
var rommaneldb = postgres.AddDatabase(name: "rommanel")
    .WithCreationScript(creationScript);

var redis = builder.AddRedis(name: "redis", password: password);

var rabbitmq = builder.AddRabbitMQ("rabbitmq", userName: username, password: password)
    .WithManagementPlugin();

var clienteApi = builder
    .AddProject<Projects.Rommanel_WebAPI>("cliente-api")
    .WithReference(rommaneldb)
    .WithReference(redis)
    .WithReference(rabbitmq)
    .WithExternalHttpEndpoints();

builder.AddNpmApp("front-end", "../webapp/rommanel-angular-webapp")
    .WithReference(clienteApi)
    .WaitFor(clienteApi)
    .WithHttpEndpoint(env: "PORT", port: 4200)
    .WithExternalHttpEndpoints();

builder.Build().Run();