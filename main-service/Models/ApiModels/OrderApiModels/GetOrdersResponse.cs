using main_service.Models.DtoModels;

namespace main_service.Models.ApiModels.OrderApiModels;

public class GetOrdersResponse
{
    public int TotalOrders { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public string Sort { get; set; } = null!;
    public List<OrderDto> Orders { get; set; } = null!;
}