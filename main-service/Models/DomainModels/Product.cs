using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;


namespace main_service.Models.DomainModels;

[Index(nameof(Name), IsUnique = true)]
public class Product
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;
    
    [Required]
    [MaxLength(999)]
    public string Description { get; set; }
    
    [DefaultValue(0)]
    [Precision(14, 2)]
    public decimal? Price { get; set; }
    
    [DefaultValue(0)]
    [Range(0, int.MaxValue)]
    public int? Stock { get; set; } = 0;
    
    [DefaultValue(0)]
    [Range(0, int.MaxValue)]
    public int? Sold { get; set; } = 0;
    
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
    
    [DefaultValue(false)]
    public bool Disabled { get; set; }
    
    // The thumbnail / main image of the product
    // if not set, the first image in the list will be used
    public int? ImageId { get; set; }
    
    // Relations to other entities
    public List<Image> Images { get; set; } = new();
    public List<Category> Categories { get; set; } = new();
    public List<OrderItem> OrderItems { get; set; } = new();
}