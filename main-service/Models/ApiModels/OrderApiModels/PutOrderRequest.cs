using main_service.Models.DomainModels;

namespace main_service.Models.ApiModels.OrderApiModels;

public class PutOrderRequest
{
    public string? Status { get; set; }
    public List<OrderItem>? OrderItems { get; set; }
}