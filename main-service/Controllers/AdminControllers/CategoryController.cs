using AutoMapper;
using main_service.Models;
using main_service.Models.ApiModels.CategoryApiModels;
using main_service.Models.DomainModels;
using main_service.Models.DtoModels;
using main_service.RabbitMQ;
using main_service.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace main_service.Controllers.AdminControllers;

[ApiController]
[Route("api/admin/[controller]")]
public class CategoryController : BaseAdminController
{
    
    [HttpGet]
    [Route("{id:int}/details")]
    public async Task<IActionResult> GetCategoryDetails(int id)
    {
        var category = await _dbContext.Categories
            .Include(c => c.Products)
            .FirstOrDefaultAsync(c => c.Id == id);
        if (category == null)
        {
            return NotFound("Category not found");
        }
        var categoryDto = _mapper.Map<CategoryDto>(category);
        var productDtos = _mapper.Map<List<ProductDto>>(category.Products);
        var response = new GetCategoryDetailsResponse
        {
            Category = categoryDto,
            Products = productDtos,
        };
        return Ok(response);
    }
    
    [HttpPost]
    [Route("{id:int}/add-product/{productId:int}")]
    public async Task<IActionResult> AddProductToCategory(int id, int productId)
    {
        var category = await _dbContext.Categories.FindAsync(id);
        if (category == null)
        {
            return NotFound("Category not found");
        }
        var product = await _dbContext.Products.FindAsync(productId);
        if (product == null)
        {
            return NotFound("Product not found");
        }
        category.Products.Add(product);
        await _dbContext.SaveChangesAsync();
        await PublishProductToBroker(productId);
        return Ok("Product added to category");
    }
    
    [HttpPost]
    [Route("{id:int}/remove-product/{productId:int}")]
    public async Task<IActionResult> RemoveProductFromCategory(int id, int productId)
    {
        Console.WriteLine("Removing product from category");
        var category = await _dbContext.Categories
            .Include(c => c.Products)
            .FirstOrDefaultAsync(c => c.Id == id);
        if (category == null)
        {
            return NotFound("Category not found");
        }

        var product = await _dbContext.Products.FindAsync(productId);
        if (product == null)
        {
            return NotFound("Product not found");
        }
        
        category.Products.Remove(product);
        await _dbContext.SaveChangesAsync();
        await PublishProductToBroker(productId);
        return Ok("Product removed from category");
    }
    
    // BASIC CRUD OPERATIONS
    // Get All Categories
    [HttpGet]
    public async Task<IActionResult> Get(
        [FromQuery] string? search,
        [FromQuery] int? page,
        [FromQuery] int? pageSize
    )
    {
        // Get all categories from the database as a queryable
        var categories = _dbContext.Categories
            .AsSplitQuery()
            .AsQueryable();
        
        // Search
        if (search != null)
        {
            categories = categories.Where(x => x.Name.Contains(search));
        }
        
        // Pagination
        (categories, var pageResult, var pageSizeResult, var totalPages, var totalCategories) = _paginationService.ApplyPagination(categories, page, pageSize);
        
        // Get the categories as a list
        var categoryList = await categories.ToListAsync();
        
        // Response
        var response = new GetCategoriesResponse
        {
            TotalCategories = totalCategories,
            Page = pageResult,
            PageSize = pageSizeResult,
            TotalPages = totalPages,
            Search = search ?? "",
            Categories = _mapper.Map<List<CategoryDto>>(categoryList),
        };
        
        return Ok(response);
    }

    // Get Category by ID
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var category = await _dbContext.Categories.FindAsync(id);
        if (category == null)
        {
            return NotFound("Category not found");
        }
        return Ok(category);
    }

    // Create Category
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] PostCategoryRequest request)
    {
        var category = new Category
        {
            Name = request.Name,
            Description = request.Description,
        };

        await _dbContext.Categories.AddAsync(category);
        await _dbContext.SaveChangesAsync();
        return Ok(category);
    }

    // Update Category
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] PutCategoryRequest request)
    {
        var category = await _dbContext.Categories.FindAsync(id);
        if (category == null)
        {
            return NotFound("Category not found");
        }
        
        category.Name = request.Name ?? category.Name;
        category.Description = request.Description ?? category.Description;
        await _dbContext.SaveChangesAsync();
        return Ok(category);
    }

    // Delete Category
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var category = await _dbContext.Categories.FindAsync(id);
        if (category == null)
        {
            return NotFound("Category not found");
        }
        _dbContext.Categories.Remove(category);
        await _dbContext.SaveChangesAsync();
        return Ok("Category deleted");
    }
    
    private async Task PublishProductToBroker(int productId)
    {
        var product = await _dbContext.Products
            .Include(p => p.ProductDescriptions)
            .Include(p => p.Categories)
            .Include(p => p.Images)
            .FirstOrDefaultAsync(p => p.Id == productId);
        
        var productDto = _mapper.Map<ProductDto>(product);
        var deserializeProduct = productDto.ToRepresentation();
        _rabbitMqProducer.PublishProductQueue(deserializeProduct);
    }


    public CategoryController(IMapper mapper, ShopDbContext dbContext, IPaginationService paginationService, IRabbitMQProducer rabbitMqProducer) : base(mapper, dbContext, paginationService, rabbitMqProducer)
    {
    }
}