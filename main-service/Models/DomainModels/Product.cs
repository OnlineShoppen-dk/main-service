using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;


namespace main_service.Models.DomainModels;

[Index(nameof(name), IsUnique = true)]
public class Product
{
    [Key]
    public int id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string name { get; set; } = null!;
    
    [Required]
    [MaxLength(999)]
    public string description { get; set; }
    
    [DefaultValue(0)]
    [Precision(14, 2)]
    public decimal? price { get; set; }
    
    [DefaultValue(0)]
    [Range(0, int.MaxValue)]
    public int? stock { get; set; } = 0;
    
    [DefaultValue(0)]
    [Range(0, int.MaxValue)]
    public int? sold { get; set; } = 0;
    
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTimeOffset createdAt { get; set; } = DateTimeOffset.UtcNow;
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTimeOffset updatedAt { get; set; } = DateTimeOffset.UtcNow;
    
    [DefaultValue(false)]
    public bool disabled { get; set; }
    
    // The thumbnail / main image of the product
    // if not set, the first image in the list will be used
    public int? ImageId { get; set; }
    
    // Relations to other entities
    public List<Image> images { get; set; } = new();
    public List<Category> categories { get; set; } = new();
    public List<OrderItem> orderItems { get; set; } = new();
    
    // Functions
    public void ProductSale(int quantity)
    {
        sold += quantity;
        stock -= quantity;
    }
    public void ProductSaleCancel(int quantity)
    {
        sold -= quantity;
        stock += quantity;
    }
    public void ProductStockUpdate(int quantity)
    {
        stock += quantity;
    }
}