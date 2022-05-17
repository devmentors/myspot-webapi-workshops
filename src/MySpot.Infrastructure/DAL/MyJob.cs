using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MySpot.Infrastructure.DAL;

internal class MyJob : BackgroundService
{
    private readonly ILogger<MyJob> _logger;

    public MyJob(ILogger<MyJob> logger)
    {
        _logger = logger;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Doing my job...");
            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }
    }
}