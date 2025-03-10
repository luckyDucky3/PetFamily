using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PetFamily.Application.FileProvider;
using PetFamily.Infrastructure.MessageQueues;
using FileInfo = PetFamily.Application.FileProvider.FileInfo;

namespace PetFamily.Infrastructure.BackgroundServices;

public class FilesCleanerBackgroundService : BackgroundService
{
    private readonly ILogger<FilesCleanerBackgroundService> _logger;
    private readonly IMessageQueue<IEnumerable<FileInfo>> _messageQueue;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public FilesCleanerBackgroundService(
        ILogger<FilesCleanerBackgroundService> logger,
        IMessageQueue<IEnumerable<FileInfo>> messageQueue,
        IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _messageQueue = messageQueue;
        _serviceScopeFactory = serviceScopeFactory;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("FilesCleanerBackgroundService is starting");
        await using var scope = _serviceScopeFactory.CreateAsyncScope();
        
        var fileProvider = scope.ServiceProvider.GetRequiredService<IFileProvider>();
        
        while (!stoppingToken.IsCancellationRequested)
        {
            var fileInfos = await _messageQueue.ReadAsync(stoppingToken);

            foreach (var fileInfo in fileInfos)
            {
                await fileProvider.DeleteFile(fileInfo, stoppingToken);
            }
        }
    }
}