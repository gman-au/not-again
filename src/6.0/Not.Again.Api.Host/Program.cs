using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Not.Again.Api.Host.Injection;
using Not.Again.Database;

var builder =
    WebApplication
        .CreateBuilder(args);

var services =
    builder
        .Services;

var configuration =
    builder
        .Configuration;

configuration
    .AddEnvironmentVariables();

services
    .AddControllers();

services
    .AddEndpointsApiExplorer();

services
    .AddSwaggerGen();

services
    .AddNotAgainServices(configuration);

var app =
    builder
        .Build();

// Verify DB
using var scope =
    app
        .Services
        .CreateScope();

var databaseInitialiser =
    scope
        .ServiceProvider
        .GetRequiredService<NotAgainDbContext>();

databaseInitialiser
    .Database
    .EnsureCreated();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app
    .UseHttpsRedirection()
    .UseAuthorization();

app
    .MapControllers();

app
    .Run();