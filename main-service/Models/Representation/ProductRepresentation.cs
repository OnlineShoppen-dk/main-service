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
    public string createdAtDate => createdAt.ToString("yyyy-MM-dd ");
    public string createdAtTime => createdAt.ToString("HH:mm:ss");
    public DateTimeOffset updatedAt { get; set; }
    public string updatedAtDate => updatedAt.ToString("yyyy-MM-dd ");
    public string updatedAtTime => updatedAt.ToString("HH:mm:ss");
    public bool isRemoved { get; set; }
    public int? imageId { get; set; }
    public List<ImageRepresentation>? images { get; set; }
}