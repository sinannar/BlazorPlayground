using Aspirant.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

//var sqlusername = builder.AddParameter("sqlusername", secret: true);
var sqlpassword = builder.AddParameter("sqlpassword", secret: true);
var sqlserver = builder.AddSqlServer("sqlserver", password: sqlpassword)
    //.WithDataVolume()
    ;
var tododatabase = sqlserver.AddDatabase("tododatabase");

var apiapplication = builder.AddProject<Projects.ApiApplication>("apiapplication")
    .WaitFor(sqlserver)
    .WaitFor(tododatabase)
    .WithReference(sqlserver)
    .WithReference(tododatabase);

builder.AddProject<Projects.BlazorApp>("blazorapp")
    .WaitFor(apiapplication)
    .WithReference(apiapplication);

builder.Build().Run();
