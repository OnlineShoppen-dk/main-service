using main_service.Models.DtoModels;

namespace main_service.Models.ApiModels.CategoryApiModels;

public class GetCategoriesResponse
{
    public int TotalCategories { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public string Search { get; set; } = null!;
    public List<CategoryDto> Categories { get; set; } = null!;
}