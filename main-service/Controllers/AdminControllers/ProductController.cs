using main_service.Models;
using main_service.Models.ApiModels.ProductApiModels;
using main_service.Models.DomainModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    public async Task<IActionResult> Get(
        [FromQuery] string? search,
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        [FromQuery] string? sort
    )
    {
        var products = _dbContext.Products
            .AsQueryable();

        var productList = await products.ToListAsync();
        
        var response = new GetProductsResponse
        {
            TotalProducts = await products.CountAsync(),
            Page = page ?? 1,
            PageSize = pageSize ?? 25,
            Search = search ?? "",
            Sort = sort ?? "name_asc",
            Products = productList
        };
        
        return Ok(response);
    }

    // Get Product by ID
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var product = _dbContext.Products.FindAsync(id);
        return Ok(product);
    }

    // Create Product
    [HttpPost]
    public async Task<IActionResult> Post()
    {
        var product = new Product
        {
            Name = "Product 1",
            Description = "This is product 1",
            Price = 100.00m
        };
        await _dbContext.Products.AddAsync(product);
        await _dbContext.SaveChangesAsync();
        var response = await _dbContext.Products.FindAsync(product.Id);
        return Ok(response);
    }

    // Update Product
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id)
    {
        var product = await _dbContext.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound("Product not found");
        }

        product.Name = "Product 1 updated";
        product.Description = "This is product 1";
        product.Price = 100.00m;
        await _dbContext.SaveChangesAsync();
        return Ok(product);
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