using main_service.Models;
using main_service.Models.DomainModels;
using Microsoft.AspNetCore.Mvc;

namespace main_service.Controllers.AdminControllers;

[ApiController]
[Route("api/admin/[controller]")]
public class ProductController : BaseAdminController
{
    // TODO: Implement search, pagination, and ordering
    // Either copy paste into each controller or create a service to handle this which can
    // be used in each controller regardless of the model
    
    private readonly ShopDbContext _dbContext;
    
    // Get All Products
    [HttpGet]
    public IActionResult Get(
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        [FromQuery] string? search,
        [FromQuery] string? orderBy
        )
    {
        var products = _dbContext.Products
            .AsQueryable();
        
        var response = new
        {
            Total = products.Count(),
            Data = products
        };
        
        return Ok(response);
    }
    
    // Get Product by ID
    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var product = _dbContext.Products.Find(id);
        if (product == null)
        {
            return NotFound("Product not found");
        }
        return Ok(product);
    }
    
    // Create Product
    [HttpPost]
    public IActionResult Post()
    {
        // Creating products can be done here
        var product = new Product();
        // TODO: Request body should be mapped to the product object
        _dbContext.Products.Add(product);
        _dbContext.SaveChanges();
        return Ok("Product created");
    }
    
    // Update Product
    [HttpPut("{id}")]
    public IActionResult Put(int id)
    {
        // Disabling, enabling, and updating products can be done here
        return Ok("Hello World");
    }
    
    // Delete Product
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _dbContext.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound("Product not found");
        }
        _dbContext.Products.Remove(product);
        await _dbContext.SaveChangesAsync();
        return Ok("Product deleted");
    }
    
    
    public ProductController(ShopDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}