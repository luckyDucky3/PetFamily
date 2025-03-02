using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace PetFamily.Application.Volunteers.BackgroundServices;

public class HardDeleteBackgroundService : BackgroundService
{
    private readonly ILogger<HardDeleteBackgroundService> _logger;
    private readonly IVolunteersRepository _volunteersRepository;

    public HardDeleteBackgroundService(
        IVolunteersRepository volunteersRepository,
        ILogger<HardDeleteBackgroundService> logger)
    {
        _volunteersRepository = volunteersRepository;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Background task running at: {time}", DateTimeOffset.Now);
            throw new NotImplementedException();
        }
    }
}