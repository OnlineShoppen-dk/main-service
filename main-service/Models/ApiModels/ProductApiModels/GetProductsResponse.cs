using main_service.Models.DomainModels;
using main_service.Models.DtoModels;

namespace main_service.Models.ApiModels.ProductApiModels;

public class GetProductsResponse
{
    public int TotalProducts { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public string Search { get; set; } = null!;
    public string Sort { get; set; } = null!;
    public List<ProductDto> Products { get; set; } = null!;
}