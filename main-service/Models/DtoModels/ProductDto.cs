namespace main_service.Models.DtoModels;

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal? Price { get; set; }
    public int? Stock { get; set; }
    public int? Sold { get; set; }
    public DateTimeOffset CreatedAt { get; set; } 
    public string CreatedAtFormatted => CreatedAt.ToString("yyyy-MM-dd - HH:mm:ss");
    public DateTimeOffset UpdatedAt { get; set; }
    public string UpdatedAtFormatted => UpdatedAt.ToString("yyyy-MM-dd - HH:mm:ss");
    public bool Disabled { get; set; }
    public int? ImageId { get; set; }
    public List<CategoryDto> Categories { get; set; } = null!;
    public List<ImageDto> Images { get; set; } = null!;
}