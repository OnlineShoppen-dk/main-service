using main_service.Models.Representation;

namespace main_service.Models.DtoModels;

public class ImageDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string FileName { get; set; } = null!;
    public string Alt { get; set; } = null!;

    public ImageRepresentation ToRepresentation()
    {
        return new ImageRepresentation
        {
            id = Id,
            name = Name,
            fileName = FileName,
            alt = Alt
        };
    }
}