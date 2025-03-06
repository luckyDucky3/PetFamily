using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace PetFamily.Infrastructure.BackgroundServices;

public class FilesCleanerBackgroundService : BackgroundService
{
    private readonly ILogger<FilesCleanerBackgroundService> _logger;

    public FilesCleanerBackgroundService(ILogger<FilesCleanerBackgroundService> logger)
    {
        _logger = logger;
    }
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("FilesCleanerBackgroundService is starting");
        
        return Task.CompletedTask;
    }
}