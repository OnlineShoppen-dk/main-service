using main_service.Models.DomainModels;

namespace main_service.Models.ApiModels.OrderApiModels;

public class PostOrderRequest
{
    public string OrderNumber { get; set; } = null!;
    public string Status { get; set; } = null!;
    public string TransactionId { get; set; } = null!;
    public List<OrderItem> OrderItems { get; set; } = null!;
}