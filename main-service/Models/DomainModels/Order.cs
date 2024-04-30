using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace main_service.Models.DomainModels;

[Index(nameof(OrderNumber), IsUnique = true)]
public class Order
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public Guid OrderNumber { get; } = Guid.NewGuid();

    [Required] 
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    
    public string? TransactionId { get; set; }
    
    // Relations to other entities
    public List<OrderItem> OrderItems { get; set; } = new();
    
    public int? UserId { get; set; }
    public UserDetails? UserDetails { get; set; }
}

public enum OrderStatus
{
    Pending,
    Processing,
    Shipped,
    Completed,
    Cancelled,
    Refunded
}

public class OrderItem
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public int Quantity { get; set; }
    
    [Required]
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    [Required]
    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;
}