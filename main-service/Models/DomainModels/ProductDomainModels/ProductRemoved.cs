namespace main_service.Models.DomainModels.ProductDomainModels;

public class ProductRemoved
{
    public int ProductRemovedId { get; set; }
    public int ProductId { get; set; }
    public DateTimeOffset RemovedAt { get; set; } = DateTimeOffset.UtcNow;
}