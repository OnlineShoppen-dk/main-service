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
    public decimal? Price { get; set; } = 0;
    
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
    
    [Required]
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
}