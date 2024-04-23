using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace main_service.Models.DomainModels;

[Index(nameof(OrderNumber), IsUnique = true)]
public class Order
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public string OrderNumber { get; set; } = null!;
    
    [Required]
    public string Status { get; set; } = null!;
    
    [Required]
    public string TransactionId { get; set; } = null!;
    
    // Relations to other entities
    public List<OrderItem> OrderItems { get; set; } = new();
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