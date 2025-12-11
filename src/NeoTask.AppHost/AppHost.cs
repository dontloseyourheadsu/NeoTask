var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

var postgres = builder.AddPostgres("postgres")
    .WithDataVolume();
var postgresDb = postgres.AddDatabase("neotaskdb");

var apiService = builder.AddProject<Projects.NeoTask_ApiService>("apiservice")
    .WithHttpHealthCheck("/health");

builder.AddProject<Projects.NeoTask_WebApp>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(cache)
    .WaitFor(cache)
    .WithReference(apiService)
    .WaitFor(apiService)
    .WithReference(postgresDb)
    .WaitFor(postgresDb);

builder.Build().Run();
