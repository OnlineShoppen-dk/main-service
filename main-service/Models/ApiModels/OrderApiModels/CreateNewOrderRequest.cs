using main_service.Models.DomainModels;
using main_service.Models.DtoModels;

namespace main_service.Models.ApiModels.OrderApiModels;

public class CreateNewOrderRequest
{
    public int UserId { get; set; }
    public List<CreateNewOrderRequestItem> Items { get; set; } = null!;
}

public class CreateNewOrderRequestItem
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}