using main_service.Models.DomainModels;
using main_service.Models.DtoModels;

namespace main_service.Models.ApiModels.CategoryApiModels;

public class GetCategoryDetailsResponse
{
    public CategoryDto Category { get; set; } = null!;
    public List<ProductDto> Products { get; set; } = null!;
}