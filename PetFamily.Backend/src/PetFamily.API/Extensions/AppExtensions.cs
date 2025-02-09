using Microsoft.EntityFrameworkCore;
using PetFamily.Infrastructure;

namespace PetFamily.API.Extensions;

public static class AppExtensions
{
    public static async Task<WebApplication> DatabaseMigrateAsync(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicatonDbContext>();
        await dbContext.Database.MigrateAsync();

        return app;
    }
}