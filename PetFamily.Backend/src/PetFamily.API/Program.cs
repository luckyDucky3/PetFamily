using PetFamily.API.Extensions;
using PetFamily.API.Middlewares;
using PetFamily.Application;
using PetFamily.Application.Volunteers;
using PetFamily.Infrastructure;
using PetFamily.Infrastructure.Repositories;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.Debug()
    .WriteTo.Seq(builder.Configuration.GetConnectionString("Seq")!)
    .Enrich.WithThreadName()
    .Enrich.WithEnvironmentName()
    .MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Warning)
    .CreateLogger();

builder.Services.AddSerilog();

builder.Services.AddInfrastructure(builder.Configuration)
    .AddApplication();

var app = builder.Build();

app.UseExceptionMiddleware();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    await app.DatabaseMigrateAsync();
}

app.UseSerilogRequestLogging();

app.UseRouting();
app.MapControllers();

app.Run();