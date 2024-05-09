using main_service.Models.ApiModels.ProductApiModels;
using main_service.Models.DomainModels;
using main_service.Models.DomainModels.ProductDomainModels;
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
            Guid = Guid.NewGuid(),
            Name = "Create Product1",
            Description = "Create Product Description 1",
            Price = 100,
            Stock = 10,
            Sold = 0
        };
        // Act
        var product = new Product
        {
            Guid = Guid.NewGuid(),
            Stock = productRequest.Stock,
            Sold = productRequest.Sold,
        };
        var productDescription = new ProductDescription
        {
            Name = productRequest.Name,
            Description = productRequest.Description,
            Price = productRequest.Price
        };
        product.ProductDescriptions.Add(productDescription);
        dbContext.Products.Add(product);
        dbContext.SaveChanges();
        // Assert
        Assert.NotNull(product);
        Assert.Equal(productRequest.Name, product.ProductDescription.Name);
        productRequest.Name = "Create Product2";
        Assert.NotEqual(productRequest.Name, product.ProductDescription.Name);
    }
}