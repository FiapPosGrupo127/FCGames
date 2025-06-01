using FCGames.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FCGames.Tests.Shared.Database;

public class TestDatabaseDockerFixture : IDisposable
{
    public ApplicationDBContext DbContext { get; private set; }
    private readonly SqlDockerSetup _sqlDockerSetup;

    public TestDatabaseDockerFixture()
    {
        _sqlDockerSetup = new SqlDockerSetup();  // Inicia o Docker
        var serviceProvider = new ServiceCollection()
            .AddDbContext<ApplicationDBContext>(options =>
                options.UseSqlServer(_sqlDockerSetup.ConnectionString))
            .BuildServiceProvider();

        DbContext = serviceProvider.GetService<ApplicationDBContext>()!;

        DbContext.Database.EnsureDeleted();
        DbContext.Database.EnsureCreated();
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        DbContext.Dispose();
        _sqlDockerSetup.Dispose();
    }
}