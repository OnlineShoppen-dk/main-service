namespace main_service.Models.Representation;

public class ImageRepresentation
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string FileName { get; set; } = null!;
    public string Alt { get; set; } = null!; 
}