namespace main_service.Models.DtoModels;

public class ImageDto
{
    public int Id { get; set; }
    public string FileName { get; set; } = null!;
    public string Alt { get; set; } = null!; 
}