using AutoMapper;
using main_service.Models;
using main_service.Models.ApiModels.ProductApiModels;
using main_service.Models.DomainModels;
using main_service.Models.DtoModels;
using main_service.RabbitMQ;
using main_service.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace main_service.Controllers.AdminControllers;

[ApiController]
[Route("api/admin/[controller]")]
public class ProductController : BaseAdminController
{
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
            .AsSplitQuery()
            .AsQueryable();

        // Search
        if (search != null)
        {
            products = products.Where(x => x.Name.Contains(search));
        }

        // Sorting
        if (sort != null)
        {
            products = sort switch
            {
                "popularity_asc" => products.OrderBy(p => p.Sold),
                "popularity_desc" => products.OrderByDescending(p => p.Sold),
                "name_asc" => products.OrderBy(p => p.Name),
                "name_desc" => products.OrderByDescending(p => p.Name),
                "price_asc" => products.OrderBy(p => p.Price),
                "price_desc" => products.OrderByDescending(p => p.Price),
                "stock_asc" => products.OrderBy(p => p.Stock),
                "stock_desc" => products.OrderByDescending(p => p.Stock),
                _ => products.OrderBy(p => p.Id)
            };
        }

        // Pagination
        (products, var pageResult, var pageSizeResult, var totalPages, var totalProducts) =
            _paginationService.ApplyPagination(products, page, pageSize);

        // Create response
        var productList = await products.ToListAsync();

        var response = new GetProductsResponse
        {
            TotalProducts = totalProducts,
            Page = pageResult,
            PageSize = pageSizeResult,
            TotalPages = totalPages,
            Search = search ?? "",
            Sort = sort ?? "",
            Products = _mapper.Map<List<ProductDto>>(productList),
        };

        return Ok(response);
    }

    // Get Product by ID
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var product = await _dbContext.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound("Product not found");
        }

        return Ok(product);
    }

    // Create Product
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] PostProductRequest request)
    {
        var product = new Product
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            Stock = request.Stock,
            Sold = 0
        };

        await _dbContext.Products.AddAsync(product);
        await _dbContext.SaveChangesAsync();
        _rabbitMqProducer.PublishProductQueue(product);
        return Ok(product);
    }

    // Update Product
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] PutProductRequest request)
    {
        var product = await _dbContext.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound("Product not found");
        }

        product.Name = request.Name ?? product.Name;
        product.Description = request.Description ?? product.Description;
        product.Price = request.Price ?? product.Price;
        product.Stock = request.Stock ?? product.Stock;
        product.Sold = request.Sold ?? product.Sold;
        product.Disabled = request.Disabled ?? product.Disabled;
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


    public ProductController(IMapper mapper, ShopDbContext dbContext, IPaginationService paginationService, IRabbitMQProducer rabbitMqProducer) : base(mapper, dbContext, paginationService, rabbitMqProducer)
    {
    }
}