using AutoMapper;
using main_service.Models;
using main_service.Models.ApiModels.CategoryApiModels;
using main_service.Models.ApiModels.ProductApiModels;
using main_service.Models.DomainModels;
using main_service.Models.DomainModels.ProductDomainModels;
using main_service.RabbitMQ;
using main_service.Services;
using Microsoft.AspNetCore.Mvc;

namespace main_service.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : BaseController
{
    
    private readonly IProductService _productService;
    
    [HttpGet]
    [Route("create-test-data")]
    public async Task<IActionResult> CreateTestData()
    {
        // Wipe the database
        _dbContext.Products.RemoveRange(_dbContext.Products);
        _dbContext.Categories.RemoveRange(_dbContext.Categories);
        await _dbContext.SaveChangesAsync();
        
        // Amount of test data
        const int productAmount = 250;
        const int categoryAmount = 20;

        // Products
        var products = await GenerateTestProducts(productAmount);
        await _dbContext.Products.AddRangeAsync(products);
        
        // Categories
        var categories = await GenerateTestCategories(categoryAmount);
        await _dbContext.Categories.AddRangeAsync(categories);
        
        // Save changes
        await _dbContext.SaveChangesAsync();
        
        // Add products to categories randomly
        foreach (var product in products)
        {
            var random = new Random();
            var randomCategory = categories[random.Next(categories.Count)];
            var randomCategory2 = categories[random.Next(categories.Count)];
            randomCategory.Products.Add(product);
            randomCategory2.Products.Add(product);
        }
        await _dbContext.SaveChangesAsync();
        
        // Publish Products to RabbitMQ
        foreach (var product in products)
        {
            _rabbitMqProducer.PublishProductQueue(product);
        }
        return Ok("Test Data Created");
    }
    
    private async Task<List<Product>> GenerateTestProducts(int amount)
    {
        var result = new List<Product>();
        // Create a product
        for (var i = 0; i < amount; i++)
        {
            // Create a product request
            var productRequest = new PostProductRequest
            {
                Guid = Guid.NewGuid(),
                Name = "Test Product #" + i,
                Description = "Test Product Description #" + i,
                Price = 100 * i,
                Stock = 10 * i,
            };
            // Simulate a request to create a product
            var product = new Product
            {
                
                Guid = Guid.NewGuid(),
                Stock = 10,
                Sold = 0,
            };
            var productDescription = new ProductDescription
            {
                Name = productRequest.Name,
                Description = productRequest.Description,
                Price = productRequest.Price
            };
            product.ProductDescriptions.Add(productDescription);
            result.Add(product);
        }
        
        return result;
    }
    private async Task<List<Category>> GenerateTestCategories(int amount)
    {
        var result = new List<Category>();
        // Create a product
        for (var i = 0; i < amount; i++)
        {
            // Create a product request
            var categoryRequest = new PostCategoryRequest
            {
                Name = "Test Category #" + i,
                Description = "Test Category Description #" + i,
            };
            var category = new Category
            {
                Name = categoryRequest.Name,
                Description = categoryRequest.Description,
            };
            result.Add(category);
        }
        return result;
    }
    
    public TestController(IMapper mapper, ShopDbContext dbContext, IPaginationService paginationService, IRabbitMQProducer rabbitMqProducer, IProductService productService) : base(mapper, dbContext, paginationService, rabbitMqProducer)
    {
        _productService = productService;
    }
}