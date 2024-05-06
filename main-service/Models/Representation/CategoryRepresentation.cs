namespace main_service.Models.Representation;

public class CategoryRepresentation
{
    public int id { get; set; }
    public string name { get; set; } = null!;
    public string description { get; set; } = null!;
    public int totalProducts { get; set; }
}