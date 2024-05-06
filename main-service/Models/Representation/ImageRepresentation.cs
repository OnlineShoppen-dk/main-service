namespace main_service.Models.Representation;

public class ImageRepresentation
{
    public int id { get; set; }
    public string name { get; set; } = null!;
    public string fileName { get; set; } = null!;
    public string alt { get; set; } = null!; 
}