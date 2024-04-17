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
        services.AddDbContext<ShopDbContext>(options =>
            options.UseMySql(connectionString,
                new MySqlServerVersion(new Version(8, 0, 29))));
    }
}