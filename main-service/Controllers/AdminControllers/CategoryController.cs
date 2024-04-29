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


    public CategoryController(IMapper mapper, ShopDbContext dbContext, IPaginationService paginationService, IRabbitMQProducer rabbitMqProducer) : base(mapper, dbContext, paginationService, rabbitMqProducer)
    {
    }
}