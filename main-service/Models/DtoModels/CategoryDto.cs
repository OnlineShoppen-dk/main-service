using main_service.Models.Representation;

namespace main_service.Models.DtoModels;

public class CategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int TotalProducts { get; set; }

    public CategoryRepresentation ToRepresentation()
    {
        return new CategoryRepresentation
        {
            Id = Id,
            Name = Name,
            Description = Description,
            TotalProducts = TotalProducts
        };
    }
}