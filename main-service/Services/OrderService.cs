using main_service.Models;
using main_service.Models.DtoModels;

namespace main_service.Services;

public interface IOrderService
{
    public void AddTransactionIdToOrder(string transactionId, int orderId);
}

public class OrderService : IOrderService
{
    private readonly ShopDbContext _dbContext;

    /// <summary>
    /// Upon successful payment, the transaction id will be added to the order
    /// </summary>
    public void AddTransactionIdToOrder(string transactionId, int orderId)
    {
        var order = _dbContext.Orders.Find(orderId);
        if (order != null)
        {
            order.TransactionId = transactionId;
            _dbContext.SaveChanges();
        }
        else
        {
            throw new Exception("Order not found");
        }
    }

    public OrderService(ShopDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}