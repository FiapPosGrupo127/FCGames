using Microsoft.EntityFrameworkCore;

namespace FCGames.Infrastructure.Data;

public class ApplicationDBContext : DbContext
{
    private readonly string _provider;

    public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
    {
        // Detecta o provider baseado na connection string ou nas extensões
        _provider = DetectProvider();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDBContext).Assembly);
    }

    private string DetectProvider()
    {
        var connection = Database.GetDbConnection();
        return connection.GetType().Name switch
        {
            "SqlConnection" => "SqlServer",
            "NpgsqlConnection" => "PostgreSql",
            _ => "SqlServer" // Default
        };
    }

    public string GetMigrationsPath()
    {
        return _provider switch
        {
            "PostgreSql" => "Migrations/PostgreSql",
            "SqlServer" => "Migrations/SqlServer",
            _ => "Migrations/SqlServer"
        };
    }
}