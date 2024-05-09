using System.ComponentModel.DataAnnotations;
using main_service.Models.DomainModels.ProductDomainModels;
using Microsoft.EntityFrameworkCore;

namespace main_service.Models.DomainModels;

[Index(nameof(Name), IsUnique = true)]
public class Category
{
    
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;
    
    [MaxLength(999)]
    public string Description { get; set; } = null!;
    
    // Relations to other entities
    public List<Product> Products { get; set; } = new();
}