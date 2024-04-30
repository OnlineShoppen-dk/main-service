using main_service.Models;
using Microsoft.EntityFrameworkCore;

namespace main_service.test;

public class ContextGenerator
{
    public static ShopDbContext GetShopDbContext()
    {
        var options = new DbContextOptionsBuilder<ShopDbContext>()
            .UseInMemoryDatabase(databaseName: "InMemoryTestDatabase")
            .Options;
        
        var dbContext = new ShopDbContext(options);
        dbContext.Database.EnsureCreated();
        return dbContext;
    }
}