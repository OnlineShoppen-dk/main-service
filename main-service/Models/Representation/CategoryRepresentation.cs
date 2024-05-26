namespace main_service.Models.Representation;

public class CategoryRepresentation
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int TotalProducts { get; set; }
}