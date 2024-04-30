using main_service.Models.DomainModels;

namespace main_service.Models.DtoModels;

public class OrderDto
{
    public int Id { get; set; }
    public string OrderNumber { get; set; } = null!;
    public string Status { get; set; } = null!;
    public string TransactionId { get; set; } = null!;
    // Relations to other entities
    public List<OrderItemDto> OrderItems { get; set; } = new();
}

public class OrderItemDto
{
    public int Id { get; set; }
    public int Quantity { get; set; }
}