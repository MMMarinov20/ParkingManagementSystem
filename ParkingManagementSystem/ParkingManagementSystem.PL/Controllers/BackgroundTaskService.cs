using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

public class BackgroundTaskService : BackgroundService
{
    private readonly ILogger<BackgroundTaskService> _logger;

    public BackgroundTaskService(ILogger<BackgroundTaskService> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));

        // Wait until the cancellation token is signaled
        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    private void DoWork(object state)
    {
        _logger.LogInformation("Background task is running.");

        // Your background task logic goes here.
    }
}
