namespace main_service.Models.ApiModels.CategoryApiModels;

public class PutCategoryRequest
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
}