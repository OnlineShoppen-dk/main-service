namespace main_service.Models.DomainModels;

public class Order
{
    public int Id { get; set; }
    
    public string OrderNumber { get; set; } = null!;
    
    public string Status { get; set; } = null!;
    
    public string TransactionId { get; set; } = null!;
    
    
    
    // Relations to other entities
    public List<OrderItem> OrderItems { get; set; } = new();
}

public class OrderItem
{
    public int Id { get; set; }
    public int Quantity { get; set; }
    public Product Product { get; set; } = null!;
}