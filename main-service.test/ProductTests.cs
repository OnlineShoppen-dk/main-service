using main_service.Models.ApiModels.ProductApiModels;
using main_service.Models.DomainModels;
using Xunit.Abstractions;

namespace main_service.test;

/// <summary>
/// This class contains tests for the Product model. This is how testings are done in C#.
/// </summary>
public class ProductTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public ProductTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void CreateProduct()
    {
        // Arrange
        var dbContext = ContextGenerator.GetShopDbContext();
        var productRequest = new PostProductRequest
        {
            Name = "Create Product1",
            Description = "Create Product Description 1"
        };
        // Act
        var product = new Product
        {
            Name = productRequest.Name,
            Description = productRequest.Description
        };
        dbContext.Products.Add(product);
        dbContext.SaveChanges();
        // Assert
        Assert.NotNull(product);
        Assert.Equal(productRequest.Name, product.Name);
        productRequest.Name = "Create Product2";
        Assert.NotEqual(productRequest.Name, product.Name);
    }

    [Fact]
    public void UpdateProduct()
    {
        // Arrange
        var dbContext = ContextGenerator.GetShopDbContext();
        var productRequest = new PostProductRequest
        {
            Name = "Update Product1",
            Description = "Update Product Description 1"
        };
        var product = new Product
        {
            Name = productRequest.Name,
            Description = productRequest.Description
        };
        dbContext.Products.Add(product);
        dbContext.SaveChanges();
        
        // Act
        productRequest.Name = "Update Product2";
        product.Name = productRequest.Name;
        dbContext.SaveChanges();
        
        // Assert
        Assert.NotNull(product);
        Assert.Equal(productRequest.Name, product.Name);
    }
}