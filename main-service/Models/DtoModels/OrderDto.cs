using main_service.Models.DomainModels;
using main_service.Models.Representation;

namespace main_service.Models.DtoModels;

public class OrderDto
{
    public int Id { get; set; }
    public string OrderNumber { get; set; } = null!;
    public string Status { get; set; } = null!;

    public string TransactionId { get; set; } = null!;

    // Relations to other entities
    public List<OrderItemDto> OrderItems { get; set; } = new();

    public OrderRepresentation ToRepresentation()
    {
        return new OrderRepresentation
        {
            Id = Id,
            OrderNumber = OrderNumber,
            Status = Status,
            TransactionId = TransactionId,
            OrderItems = OrderItems.Select(x => x.ToRepresentation()).ToList()
        };
    }
}

public class OrderItemDto
{
    public int Id { get; set; }
    public int Quantity { get; set; }

    public OrderItemRepresentation ToRepresentation()
    {
        return new OrderItemRepresentation
        {
            id = Id,
            quantity = Quantity
        };
    }
}