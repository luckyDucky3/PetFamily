using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PetFamily.API;
using PetFamily.Infrastructure;
using PetFamily.Infrastructure.DbContexts;
using Testcontainers.PostgreSql;

namespace Petfamily.Application.IntegrationTests.Pets;

public class IntegrationTestWebFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres")
        .WithDatabase("PetFamily_tests")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        ChangeConfiguration(builder);
        
        builder.ConfigureTestServices(ConfigureDefaultServices);
    }

    protected virtual void ConfigureDefaultServices(IServiceCollection services)
    {
        services.AddScoped<WriteDbContext>(provider =>
        {
            var configuration = provider.GetRequiredService<IConfiguration>();
            return new WriteDbContext(configuration);
        });
        
        services.AddScoped<ReadDbContext>(provider =>
        {
            var configuration = provider.GetRequiredService<IConfiguration>();
            return new ReadDbContext(configuration);
        });
    }

    private void ChangeConfiguration(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(configBuilder =>
        {
            configBuilder.AddInMemoryCollection(new Dictionary<string, string?>
            {
                [Constants.DATABASE] = _dbContainer.GetConnectionString()
            });
        });
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<WriteDbContext>();
        await dbContext.Database.EnsureCreatedAsync();
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
        await _dbContainer.DisposeAsync();
    }
}