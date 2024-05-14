using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace main_service.Models.DomainModels.ProductDomainModels;

[Index(nameof(ProductId), IsUnique = true)]
public class ProductRemoved
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public DateTimeOffset RemovedAt { get; set; } = DateTimeOffset.UtcNow;
    
    [Required]
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
}