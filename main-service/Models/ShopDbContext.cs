using main_service.Models.DomainModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace main_service.Models;


/// <summary>
/// This is primarily based off the official documentation on microsoft
/// Link: https://learn.microsoft.com/en-us/ef/core/
/// TODO: This has not been implemented yet, but i have put down some of the ways to make things work
/// </summary>
public class ShopDbContext : DbContext
{
    public ShopDbContext(DbContextOptions<ShopDbContext> options) : base(options)
    {
    }
    /// <summary>
    /// From my understanding this was only needed during production
    /// It was necessary for when migration were being made
    /// TODO: So whether this should be pushed, or ignored im unsure of, maybe put into a separate class?
    /// </summary>
    public class ShopDbContextFactory : IDesignTimeDbContextFactory<ShopDbContext>
    {
        public ShopDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ShopDbContext>();
            optionsBuilder.UseMySql("server=main-service-db;user=user;password=userpass;database=mainservicedb",
                new MySqlServerVersion(new Version(8, 0, 29)));
            return new ShopDbContext(optionsBuilder.Options);
        }
    }

    // here will all the models be set, an example:
    // public DbSet<MODEL_NAME> MODEL_NAMES { get; set; } = null!;
    
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<OrderItem> OrderItems { get; set; } = null!;

    
    /// <summary>
    /// This is used to create relations, indexes etc.
    /// This needs to be correct and documented since it will be applied unto database upon migration
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Some examples
        /*
        - - To make one of the models value unique
        modelBuilder.Entity<User>()
            .HasIndex(e => e.Email)
            .IsUnique();
            
        - - Relations
        # ONE TO ONE
         modelBuilder.Entity<User>()
            .HasOne(e => e.Cart)
            .WithOne(e => e.User)
            .HasForeignKey<Cart>(e => e.UserId)
            .IsRequired();
        # MANY TO ONE
         modelBuilder.Entity<User>()
            .HasMany(e => e.Orders)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserId);
        # MANY TO MANY
        modelBuilder.Entity<Product>()
            .HasMany(e => e.Categories)
            .WithMany(e => e.Products);
            
        - - Ownership is also possible, if for example a user can have many addresses
        modelBuilder.Entity<User>()
            .OwnsMany(e => e.Addresses);
         */
        
    }
}