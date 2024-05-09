using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using main_service.Models.DtoModels;
using Microsoft.EntityFrameworkCore;

namespace main_service.Models.DomainModels.ProductDomainModels;

[Index(nameof(Guid), IsUnique = true)]
public class Product
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public Guid Guid { get; set; } = Guid.NewGuid();
    
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    
    [DefaultValue(0)]
    [Range(0, int.MaxValue)]
    public int? Stock { get; set; } = 0;
    
    [DefaultValue(0)]
    [Range(0, int.MaxValue)]
    public int? Sold { get; set; } = 0;

    public bool IsRemoved => ProductRemoved != null;
    
    // Relations to other entities
    public ProductDescription ProductDescription => ProductDescriptions.MaxBy(pd => pd.UpdatedAt) ?? new ProductDescription();
    public List<ProductDescription> ProductDescriptions { get; set; } = new();
    
    public int? ProductRemovedId { get; set; }
    public ProductRemoved? ProductRemoved { get; set; }
    
    public List<Image> Images { get; set; } = new();
    public List<Category> Categories { get; set; } = new();
    public List<OrderItem> OrderItems { get; set; } = new();

    public ProductDto ConvertToDto(List<CategoryDto> categories, List<ImageDto> images)
    {
        var productDto = new ProductDto
        {
            Id = Id,
            Guid = Guid,
            Name = ProductDescription.Name,
            Description = ProductDescription.Description,
            Price = ProductDescription.Price,
            Stock = Stock,
            Sold = Sold,
            CreatedAt = CreatedAt,
            IsRemoved = IsRemoved,
            // ProductDescription Infos
            UpdatedAt = ProductDescription.UpdatedAt,
            // Relations
            Categories = categories,
            Images = images
        };
        return productDto;
    }
    
    public void ChangeStock(int amount)
    {
        Stock += amount;
    }
}