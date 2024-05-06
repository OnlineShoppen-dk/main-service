
using AutoMapper;
using main_service.Models;
using main_service.Models.DomainModels;
using main_service.RabbitMQ;
using main_service.Services;
using Microsoft.AspNetCore.Mvc;

namespace main_service.Controllers;

/// <summary>
/// Test Controller
/// Test functions which are not related to the main functionality, but are used for testing purposes
/// </summary>
[ApiController]
[Route("api/test/[controller]")]
public class TestController : BaseController
{

    [HttpGet]
    [Route("create-test-data")]
    public async Task<IActionResult> CreateTestData()
    {
        // Remove all existing data
        _dbContext.Categories.RemoveRange(_dbContext.Categories);
        _dbContext.Products.RemoveRange(_dbContext.Products);
        await _dbContext.SaveChangesAsync();
        // Count for each type of test data
        const int categoryCount = 15;
        const int productCount = 250;
        
        // Create Test Categories
        var categoryList = new List<Category>();
        for (int i = 0; i < categoryCount; i++)
        {
            var category = new Category
            {
                Name = $"Category {i + 1}",
                Description = $"Category Description {i + 1}"
            };
            categoryList.Add(category);
        }
        _dbContext.Categories.AddRange(categoryList);
        await _dbContext.SaveChangesAsync();
       
        // Create Test Products
        var productList = new List<Product>();
        for (int i = 0; i < productCount; i++)
        {
            var product = new Product
            {
                name = $"Product {i + 1}",
                description = $"Product Description {i + 1}",
                price = 1000,
                stock = 100,
            };
            productList.Add(product);
        }
        _dbContext.Products.AddRange(productList);
        await _dbContext.SaveChangesAsync();
        return Ok("Test Data Created");

    }
    
    [HttpGet]
    [Route("delete-test-data")]
    public async Task<IActionResult> DeleteTestData()
    {
        // Remove all existing data
        _dbContext.Categories.RemoveRange(_dbContext.Categories);
        _dbContext.Products.RemoveRange(_dbContext.Products);
        await _dbContext.SaveChangesAsync();
        return Ok("Test Data Deleted");
    }

    public TestController(IMapper mapper, ShopDbContext dbContext, IPaginationService paginationService, IRabbitMQProducer rabbitMqProducer) : base(mapper, dbContext, paginationService, rabbitMqProducer)
    {
    }
}