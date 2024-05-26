namespace main_service.Models.Representation;

public class OrderRepresentation
{
    public int Id { get; set; }
    public string OrderNumber { get; set; } = null!;
    public string Status { get; set; } = null!;
    public string TransactionId { get; set; } = null!;
    // Relations to other entities
    public List<OrderItemRepresentation> OrderItems { get; set; } = new();
}

public class OrderItemRepresentation
{
    public int Id { get; set; }
    public int Quantity { get; set; }
}