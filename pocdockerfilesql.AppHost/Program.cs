var builder = DistributedApplication.CreateBuilder(args);

var sqldb = builder.AddSqlServer("sql")
    .WithDataVolume()
    .AddDatabase("sqldb");

var apiService = builder.AddProject<Projects.pocdockerfilesql_ApiService>("apiservice")
    .WithReference(sqldb);

builder.AddProject<Projects.pocdockerfilesql_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);

builder.Build().Run();
