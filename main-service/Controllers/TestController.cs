using AutoMapper;
using main_service.Models;
using main_service.Models.ApiModels.CategoryApiModels;
using main_service.Models.DomainModels;
using main_service.Models.DomainModels.ProductDomainModels;
using main_service.RabbitMQ;
using main_service.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace main_service.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : BaseController
{
    
    private readonly IProductService _productService;
    
    /// <summary>
    /// Adds 250 products and 25 categories to the database
    /// 250 products are published to RabbitMQ as well, and thereafter saved on ElasticSearch
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("create-test-data")]
    public async Task<IActionResult> CreateTestData()
    {
        // Wipe the database
        _dbContext.Products.RemoveRange(_dbContext.Products);
        _dbContext.Categories.RemoveRange(_dbContext.Categories);
        
        // Amount of test data
        const int productAmount = 250;
        const int categoryAmount = 25;

        // Categories
        await GenerateTestCategories(categoryAmount);
        
        // Products
        await GenerateTestProducts(productAmount);
        
        // Publish Products to RabbitMQ
        return Ok("Test Data Created");
    }
    
    private async Task GenerateTestProducts(int amount)
    {
        // Create a product
        for (var i = 0; i < amount; i++)
        {
            // Simulate a request to create a product
            var product = new Product
            {
                Guid = Guid.NewGuid(),
                Stock = 100 * i,
                Sold = 0,
            };
            var productDescription = new ProductDescription
            {
                Name = "Test Product #" + i,
                Description = "Test Product Description #" + i,
                Price = 100 * i,
            };
            product.ProductDescriptions.Add(productDescription);
            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync();
        }

        var result = await _dbContext.Products.ToListAsync();
        Console.WriteLine("Products fetched");
        foreach (var product in result)
        {
            var productDto = _productService.ConvertToDto(product);
            var deserializeProduct = productDto.ToRepresentation();
            _rabbitMqProducer.PublishProductQueue(deserializeProduct);
        }
    }
    private async Task GenerateTestCategories(int amount)
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
        await _dbContext.Categories.AddRangeAsync(result);
        await _dbContext.SaveChangesAsync();
    }
    
    public TestController(IMapper mapper, ShopDbContext dbContext, IPaginationService paginationService, IRabbitMQProducer rabbitMqProducer, IProductService productService) : base(mapper, dbContext, paginationService, rabbitMqProducer)
    {
        _productService = productService;
    }
}