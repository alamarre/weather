var builder = DistributedApplication.CreateBuilder(args);
var postgres = builder.AddPostgres("postgres");
var pgdb =  postgres.AddDatabase("main");

var sqlServer = builder.AddSqlServer("sqlserver");
var sqldb = sqlServer.AddDatabase("master");

if(builder.Configuration["EPHEMERAL"] != "true")
{
    postgres.WithPgAdmin()
        .WithLifetime(ContainerLifetime.Persistent);
    sqlServer
        .WithLifetime(ContainerLifetime.Persistent);
}

if (builder.Configuration["TESTING"] != "true")
{
     builder.AddProject<Projects.Weather_Api_Core>("WeatherAPI")
        .WithReference(pgdb)
        .WithReference(sqldb);
}

builder.Build().Run();
