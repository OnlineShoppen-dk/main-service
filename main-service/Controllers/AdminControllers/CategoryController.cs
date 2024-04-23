using AutoMapper;
using main_service.Models;
using main_service.Models.ApiModels.CategoryApiModels;
using main_service.Models.DomainModels;
using main_service.Models.DtoModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace main_service.Controllers.AdminControllers;

[ApiController]
[Route("api/admin/[controller]")]
public class CategoryController : BaseAdminController
{
    // TODO: Implement search, pagination, and ordering
    // Either copy paste into each controller or create a service to handle this which can
    // be used in each controller regardless of the model

    // Get All Categorys
    [HttpGet]
    public async Task<IActionResult> Get(
        [FromQuery] string? search,
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        [FromQuery] string? sort
    )
    {

        var categories = _dbContext.Categories
            .AsSplitQuery()
            .AsQueryable();

        if (search != null)
        {
            categories = categories.Where(x => x.Name.Contains(search));
        }
        
        // Sorting
        
        // Pagination
        const int defaultPage = 1;
        const int defaultPageSize = 25;
        
        // Create response
        var categoryList = await categories.ToListAsync();
        var totalCategories = categoryList.Count;
        
        var response = new GetCategoriesResponse
        {
            TotalCategories = totalCategories,
            Page = page ?? defaultPage,
            PageSize = pageSize ?? defaultPageSize,
            Search = search ?? "",
            Sort = sort ?? "",
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

    public CategoryController(IMapper mapper, ShopDbContext dbContext) : base(mapper, dbContext)
    {
    }
}