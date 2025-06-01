using FCGames.Infrastructure.Data;
using FCGames.Tests.Shared.Database;

namespace FCGames.Tests.Domain.Services;

public abstract class BaseServiceTests : IDisposable
{
    protected ApplicationDBContext _context;
    private readonly TestDatabaseMemoryFixture _dbFixture;

    protected BaseServiceTests()
    {
        _dbFixture = new TestDatabaseMemoryFixture();
        _context = _dbFixture.Context;
    }

    protected async Task SaveChanges()
    {
        await _context.SaveChangesAsync();
    }

    public virtual void Dispose()
    {
        _context?.Dispose();
        _dbFixture.Dispose();
        GC.SuppressFinalize(this);
    }
}