using PetFamily.Application;
using PetFamily.Application.Volunteers;
using PetFamily.Application.Volunteers.CreateVolunteers;
using PetFamily.Infrastructure;
using PetFamily.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddInfrastructure()
    .AddApplication();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.Run();