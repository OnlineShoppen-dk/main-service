namespace main_service.Models.ApiModels.ProductApiModels;

public class PutProductRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal? Price { get; set; }
    public int? Stock { get; set; }
    public int? Sold { get; set; }
    public bool? Disabled { get; set; }
}