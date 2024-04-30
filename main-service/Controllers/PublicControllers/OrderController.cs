using AutoMapper;
using main_service.Models;
using main_service.Models.ApiModels.OrderApiModels;
using main_service.Models.DomainModels;
using main_service.Models.DtoModels;
using main_service.RabbitMQ;
using main_service.Services;
using Microsoft.AspNetCore.Mvc;

namespace main_service.Controllers.PublicControllers;


/// <summary>
/// Orders can be initiated by users, regardless of being logged in or not.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class OrderController : BaseController
{
    
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateNewOrderRequest request)
    {
        // Find the products in the request, and create order items
        var orderItems = request.Items.Select(i => new OrderItem
        {
            ProductId = i.ProductId,
            Quantity = i.Quantity
        }).ToList();

        // Find user
        var order = new Order
        {
            OrderItems = orderItems,
        };

        var user = await _dbContext.UserDetails.FindAsync(request.UserId);
        if (user != null)
        {
            user.Orders.Add(order);
            await _dbContext.SaveChangesAsync();
        }
        else
        {
            await _dbContext.Orders.AddAsync(order);
            await _dbContext.SaveChangesAsync();
        }

        var orderDto = _mapper.Map<OrderDto>(order);
        return Ok(orderDto);
    }
    
    public OrderController(IMapper mapper, ShopDbContext dbContext, IPaginationService paginationService, IRabbitMQProducer rabbitMqProducer) : base(mapper, dbContext, paginationService, rabbitMqProducer)
    {
    }
}