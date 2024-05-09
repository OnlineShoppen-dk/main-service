using main_service.Models;
using main_service.Models.DomainModels;
using main_service.Models.DtoModels;
using Microsoft.EntityFrameworkCore;

namespace main_service.Services;

public interface IOrderService
{
    public void AddTransactionIdToOrder(string transactionId, int orderId);
    public void UpdateOrderStatus(int orderId, OrderStatus status);
}

public class OrderService : IOrderService
{
    private readonly ShopDbContext _dbContext;

    /// <summary>
    /// Upon successful transaction, items will be deducted from stock.
    /// </summary>
    public void AddTransactionIdToOrder(string transactionId, int orderId)
    {
        var order = _dbContext.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .FirstOrDefault(o => o.Id == orderId);
        if(order == null)
        {
            throw new Exception("Order not found");
        }
        order.TransactionId = transactionId;
        // TODO: Deduct items from stock
        foreach (var orderItem in order.OrderItems)
        {
            orderItem.Product.ChangeStock(-orderItem.Quantity);
        }
        _dbContext.Orders.Update(order);
        _dbContext.SaveChanges();
    }

    /// <summary>
    /// This method is used to update the status of an order, for example, when it is shipped, or transaction status updates.
    /// </summary>
    public void UpdateOrderStatus(int orderId, OrderStatus status)
    {
        var order = _dbContext.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .FirstOrDefault(o => o.Id == orderId);
        if(order == null)
        {
            throw new Exception("Order not found");
        }
        order.Status = status;
        if(status == OrderStatus.Cancelled)
        {
            foreach (var orderItem in order.OrderItems)
            {
                orderItem.Product.ChangeStock(orderItem.Quantity);
            }
        }
        _dbContext.Orders.Update(order);
        _dbContext.SaveChanges();
    }
    
    

    public OrderService(ShopDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}