using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MySpot.Core.Abstractions;
using MySpot.Core.Entities;
using MySpot.Core.ValueObjects;

namespace MySpot.Infrastructure.DAL;

internal class DatabaseInitializer : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IClock _clock;

    public DatabaseInitializer(IServiceProvider serviceProvider, IClock clock)
    {
        _serviceProvider = serviceProvider;
        _clock = clock;
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MySpotDbContext>();
        await dbContext.Database.MigrateAsync(cancellationToken);

        if (await dbContext.WeeklyParkingSpots.AnyAsync(cancellationToken))
        {
            return;
        }

        var parkingSpots = new List<WeeklyParkingSpot>
        {
            WeeklyParkingSpot.Create(Guid.NewGuid(), new Week(_clock.Current()), "P1"),
            WeeklyParkingSpot.Create(Guid.NewGuid(), new Week(_clock.Current()), "P2"),
            WeeklyParkingSpot.Create(Guid.NewGuid(), new Week(_clock.Current()), "P3"),
            WeeklyParkingSpot.Create(Guid.NewGuid(), new Week(_clock.Current()), "P4"),
            WeeklyParkingSpot.Create(Guid.NewGuid(), new Week(_clock.Current()), "P5")
        };

        await dbContext.WeeklyParkingSpots.AddRangeAsync(parkingSpots, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}