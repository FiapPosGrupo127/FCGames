using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace FCGames.Infrastructure.Data;

public class SqlServerDbContextFactory : IDesignTimeDbContextFactory<ApplicationDBContext>
{
    public ApplicationDBContext CreateDbContext(string[] args)
    {
        // Carrega a configuração do projeto API
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../FCGames.API"))
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .Build();

        var options = new DbContextOptionsBuilder<ApplicationDBContext>();
        
        var connectionString = configuration.GetConnectionString("SqlServer");
        options.UseSqlServer(connectionString, x => 
        {
            x.MigrationsHistoryTable("__EFMigrationsHistory", "dbo");
        });
        
        Console.WriteLine("Using SqlServer provider for migrations");
        return new ApplicationDBContext(options.Options);
    }
}
