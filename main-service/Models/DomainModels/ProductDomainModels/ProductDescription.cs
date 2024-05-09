using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace main_service.Models.DomainModels.ProductDomainModels;

public class ProductDescription
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;

    [Required] 
    [MaxLength(999)] 
    public string Description { get; set; } = null!;
    
    [DefaultValue(0)]
    [Precision(14, 2)]
    public decimal? Price { get; set; }
    
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
    
    // The thumbnail / main image of the product
    public int? ImageId { get; set; }
    
    // Relations to other entities
    public List<Image> Images { get; set; } = new();
    public List<Category> Categories { get; set; } = new();
    public List<OrderItem> OrderItems { get; set; } = new();
}