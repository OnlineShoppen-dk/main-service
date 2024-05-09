using main_service.Models.Representation;

namespace main_service.Models.DtoModels;

public class ProductDto
{
    public int Id { get; set; }
    public Guid Guid { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal? Price { get; set; }
    public int? Stock { get; set; }
    public int? Sold { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public string CreatedAtDate => CreatedAt.ToString("yyyy-MM-dd ");
    public string CreatedAtTime => CreatedAt.ToString("HH:mm:ss");
    public DateTimeOffset UpdatedAt { get; set; }
    public string UpdatedAtDate => UpdatedAt.ToString("yyyy-MM-dd ");
    public string UpdatedAtTime => UpdatedAt.ToString("HH:mm:ss");
    public bool Disabled { get; set; }
    public int? ImageId { get; set; }
    public List<CategoryDto> Categories { get; set; } = null!;
    public List<ImageDto> Images { get; set; } = null!;

    public ProductRepresentation ToRepresentation()
    {
        return new ProductRepresentation
        {
            id = Id,
            name = Name,
            description = Description,
            price = Price,
            stock = Stock,
            sold = Sold,
            createdAt = CreatedAt,
            updatedAt = UpdatedAt,
            disabled = Disabled,
            imageId = ImageId,
            images = Images.Select(i => i.ToRepresentation()).ToList()
        };
    }
}