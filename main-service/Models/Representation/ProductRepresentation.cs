namespace main_service.Models.Representation;

public class ProductRepresentation
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal? Price { get; set; }
    public int? Stock { get; set; }
    public int? Sold { get; set; }
    public DateTimeOffset CreatedAt { get; set; } 
    public DateTimeOffset UpdatedAt { get; set; }
    public bool Disabled { get; set; }
    public List<ImageRepresentation>? Images { get; set; }
    public List<CategoryRepresentation>? Categories { get; set; }
}