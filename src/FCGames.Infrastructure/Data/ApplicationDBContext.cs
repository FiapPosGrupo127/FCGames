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
        try
        {
            // Verifica se é um provider relacional antes de tentar acessar a conexão
            if (Database.IsRelational())
            {
                var connection = Database.GetDbConnection();
                return connection.GetType().Name switch
                {
                    "SqlConnection" => "SqlServer",
                    "NpgsqlConnection" => "PostgreSql",
                    _ => "SqlServer" // Default
                };
            }
            else
            {
                // Para providers não relacionais (como InMemory nos testes)
                return "InMemory";
            }
        }
        catch
        {
            // Fallback em caso de erro
            return "SqlServer";
        }
    }

    public string GetMigrationsPath()
    {
        return _provider switch
        {
            "PostgreSql" => "Migrations/PostgreSql",
            "SqlServer" => "Migrations/SqlServer",
            "InMemory" => "Migrations/InMemory", // Para testes
            _ => "Migrations/SqlServer"
        };
    }
}