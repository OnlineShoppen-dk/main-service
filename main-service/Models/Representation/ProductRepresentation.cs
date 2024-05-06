namespace main_service.Models.Representation;

public class ProductRepresentation
{
    public int id { get; set; }
    public string name { get; set; } = null!;
    public string description { get; set; } = null!;
    public decimal? price { get; set; }
    public int? stock { get; set; }
    public int? sold { get; set; }
    public DateTimeOffset createdAt { get; set; } 
    public string createdAtFormatted => createdAt.ToString("yyyy-MM-dd - HH:mm:ss");
    public DateTimeOffset updatedAt { get; set; }
    public string updatedAtFormatted => updatedAt.ToString("yyyy-MM-dd - HH:mm:ss");
    public bool disabled { get; set; }
    public int? imageId { get; set; }
    public List<ImageRepresentation>? images { get; set; }
}