using System;
using Microsoft.EntityFrameworkCore;
using MySpot.Infrastructure.DAL;

namespace MySpot.Tests.Integration;

internal class TestDatabase : IDisposable
{
    public MySpotDbContext Context { get; }

    public TestDatabase()
    {
        var connectionString = $"Host=localhost;Database=MySpot-tests-{Guid.NewGuid()};Username=postgres;Password=";
        Context = new MySpotDbContext(
            new DbContextOptionsBuilder<MySpotDbContext>()
                .UseNpgsql(connectionString).Options);
    }

    public void Dispose()
    {
        Context?.Database.EnsureDeleted();
        Context?.Dispose();
    }
}