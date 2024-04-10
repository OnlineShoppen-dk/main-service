using main_service.Models;
using Microsoft.EntityFrameworkCore;

namespace main_service.Configurations;

/// <summary>
/// This will be used to configure the database for the application
/// </summary>
public static class DatabaseConfig
{
    public static void Configure(IServiceCollection services, string connectionString)
    {
        
        // TODO: This needs to be implemented, below are examples of how to configure the database, we need to know which database is being used
        /*
        // Database: SqlServer
        services.AddDbContext<DbContext>(options =>
            options.UseSqlServer(connectionString));
            
        // Database: Postgres
        services.AddDbContext<DbContext>(options =>
            options.UseNpgsql(connectionString));
        
        // Database: MySql
        services.AddDbContext<DbContext>(options =>
            options.UseMySql(connectionString,
                new MySqlServerVersion(new Version(8, 0, 29))));
        */
        Console.WriteLine("Database configured");
        Console.WriteLine("Connection string: " + connectionString);
        services.AddDbContext<ShopDbContext>(options =>
            options.UseMySql(connectionString,
                new MySqlServerVersion(new Version(8, 0, 29))));
    }
}