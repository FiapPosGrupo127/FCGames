using FCGames.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FCGames.Tests.Shared.Database;

public class TestDatabaseMemoryFixture
{
    public ApplicationDBContext Context { get; private set; }

    public TestDatabaseMemoryFixture()
    {
        var options = new DbContextOptionsBuilder<ApplicationDBContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        Context = new ApplicationDBContext(options);

        Context.Database.EnsureDeleted();
        Context.Database.EnsureCreated();
    }

    public void Dispose()
    {
        Context.Database.EnsureDeleted();
        Context.Dispose();
    }
}