using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Minio;
using Minio.Credentials;
using PetFamily.Application.Database;
using PetFamily.Application.Species;
using PetFamily.Application.Volunteers;
using PetFamily.Infrastructure.Options;
using PetFamily.Infrastructure.Providers;
using PetFamily.Infrastructure.Repositories;
using IFileProvider = PetFamily.Application.FileProvider.IFileProvider;

namespace PetFamily.Infrastructure;

public static class Inject
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<ApplicatonDbContext>();
        services.AddScoped<IVolunteersRepository, VolunteersRepository>();
        services.AddScoped<ISpeciesRepository, SpeciesRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddMinio(configuration);

        return services;
    }

    private static IServiceCollection AddMinio(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<MinioOptions>(configuration.GetSection(MinioOptions.MINIO));
        services.AddMinio(options =>
        {
            var minioOptions = configuration.GetSection(MinioOptions.MINIO).Get<MinioOptions>()
                               ?? throw new ApplicationException("Missing minio configuration");
            var endpoint = minioOptions.Endpoint;
            options.WithEndpoint(endpoint);
            options.WithCredentials(minioOptions.AccessKey, minioOptions.SecretKey);
            options.WithSSL(minioOptions.WithSSL);
        });
        services.AddScoped<IFileProvider, MinioProvider>();
        
        return services;
    }
}