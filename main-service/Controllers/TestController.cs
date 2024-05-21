using AutoMapper;
using main_service.Models;
using main_service.Models.DomainModels;
using main_service.Models.DomainModels.ProductDomainModels;
using main_service.Models.DtoModels;
using main_service.RabbitMQ;
using main_service.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace main_service.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : BaseController
{
    [HttpGet]
    [Route("setup")]
    public async Task<IActionResult> Setup()
    {
        // Delete all data
        _dbContext.RemoveRange(_dbContext.Products);
        _dbContext.RemoveRange(_dbContext.Categories);
        _dbContext.RemoveRange(_dbContext.UserDetails);
        await _dbContext.SaveChangesAsync();

        // Create test data
        const int productCount = 10;
        const int categoryCount = 5;

        // Create test products
        var productsCreated = await CreateTestProducts(productCount);
        // Create test categories
        var categoriesCreated = await CreateTestCategories(categoryCount);
        // Create test user
        var usersCreated = await CreateTestUser();

        // Validated
        if (!productsCreated || !categoriesCreated || !usersCreated)
        {
            return BadRequest("Setup failed");
        }

        // Add products to categories
        var productsAddedToCategories = await AddTestProductsToCategories();
        if (!productsAddedToCategories)
        {
            return BadRequest("Setup failed 2");
        }
        
        // Publish to rabbitmq
        var productsPublished = await PublishTestProductsToRabbitMQ();
        if (!productsPublished)
        {
            return BadRequest("Setup failed 3");
        }

        return Ok("Setup done");
    }


    private async Task<bool> CreateTestProducts(int productCount)
    {
        var products = new List<Product>();
        for (var i = 0; i < productCount; i++)
        {
            var product = new Product
            {
                Guid = Guid.NewGuid(),
                Stock = i * 10,
            };
            var productDescription = new ProductDescription
            {
                Name = $"Product {i}",
                Description = $"Description {i}",
                Price = i * 100,
            };
            product.ProductDescriptions.Add(productDescription);
            products.Add(product);
        }

        await _dbContext.Products.AddRangeAsync(products);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    private async Task<bool> CreateTestCategories(int categoryCount)
    {
        var categories = new List<Category>();
        for (var i = 0; i < categoryCount; i++)
        {
            var category = new Category
            {
                Name = $"Category {i}",
                Description = $"Description {i}",
            };
            categories.Add(category);
        }

        await _dbContext.Categories.AddRangeAsync(categories);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    private async Task<bool> CreateTestUser()
    {
        var user = new UserDetails
        {
            Guid = Guid.NewGuid(),
            FirstName = "Test",
            LastName = "User",
            Email = "user@test.com",
            PhoneNumber = "11111111",
            Address = "Test Address"
        };

        var admin = new UserDetails
        {
            Guid = Guid.NewGuid(),
            FirstName = "Test",
            LastName = "Admin",
            Email = "admin@test.com",
            PhoneNumber = "22222222",
            Address = "Admin Address"
        };
        await _dbContext.UserDetails.AddRangeAsync(user, admin);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    private async Task<bool> AddTestProductsToCategories()
    {
        var categories = await _dbContext.Categories
            .Include(x => x.Products)
            .ToListAsync();
        var products = await _dbContext.Products
            .Include(x => x.ProductDescriptions)
            .Include(x => x.Images)
            .ToListAsync();

        foreach (var category in categories)
        {
            var random = new Random();
            var randomProducts = products.OrderBy(x => random.Next()).Take(5).ToList();
            foreach (var product in randomProducts)
            {
                category.Products.Add(product);
            }
        }

        await _dbContext.SaveChangesAsync();
        return true;
    }

    private async Task<bool> PublishTestProductsToRabbitMQ()
    {
        var products = await _dbContext.Products
            .Include(x => x.ProductDescriptions)
            .Include(x => x.Images)
            .Include(x => x.Categories)
            .ToListAsync();
        foreach (var product in products)
        {
            PublishProductToBroker(product);
        }

        return true;
    }

    private void PublishProductToBroker(Product product)
    {
        var productDto = _mapper.Map<ProductDto>(product);
        var deserializeProduct = productDto.ToRepresentation();
        _rabbitMqProducer.PublishProductQueue(deserializeProduct);
    }

    public TestController(IMapper mapper, ShopDbContext dbContext, IPaginationService paginationService,
        IRabbitMQProducer rabbitMqProducer) : base(mapper, dbContext, paginationService, rabbitMqProducer)
    {
    }
}