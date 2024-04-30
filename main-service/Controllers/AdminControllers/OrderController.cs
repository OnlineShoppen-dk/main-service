using AutoMapper;
using main_service.Models;
using main_service.Models.ApiModels.OrderApiModels;
using main_service.Models.DomainModels;
using main_service.Models.DtoModels;
using main_service.RabbitMQ;
using main_service.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace main_service.Controllers.AdminControllers;

[ApiController]
[Route("api/admin/[controller]")]
public class OrderController : BaseAdminController
{
    
    
    // Basic CRUD operations for Orders
    [HttpGet]
    public async Task<IActionResult> Get(
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        [FromQuery] string? sort
    )
    {

        var orders = _dbContext.Orders
            .Include(o => o.OrderItems)
            .AsSplitQuery()
            .AsQueryable();

        
        // Sorting
        if (sort != null)
        {
            orders = sort switch
            {
                "order_number_asc" => orders.OrderBy(o => o.OrderNumber),
                "order_number_desc" => orders.OrderByDescending(o => o.OrderNumber),
                "status_asc" => orders.OrderBy(o => o.Status),
                "status_desc" => orders.OrderByDescending(o => o.Status),
                "transaction_id_asc" => orders.OrderBy(o => o.TransactionId),
                "transaction_id_desc" => orders.OrderByDescending(o => o.TransactionId),
                _ => orders.OrderBy(o => o.Id)
            };
        }
        
        // Pagination
        (orders, var pageResult, var pageSizeResult, var totalPages, var totalOrders) = _paginationService.ApplyPagination(orders, page, pageSize);        
        // Create response
        var orderList = await orders.ToListAsync();
        
        var response = new GetOrdersResponse
        {
            TotalOrders = totalOrders,
            Page = pageResult,
            PageSize = pageSizeResult,
            TotalPages = totalPages,
            Sort = sort ?? "",
            Orders = _mapper.Map<List<OrderDto>>(orderList),
        };
        
        return Ok(response);
    }

    // Get Order by ID
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var order = await _dbContext.Orders.FindAsync(id);
        if (order == null)
        {
            return NotFound("Order not found");
        }
        return Ok(order);
    }

    // Create Order
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] PostOrderRequest request)
    {
        var order = new Order
        {
            OrderItems = request.OrderItems,
            TransactionId = request.TransactionId
        };

        await _dbContext.Orders.AddAsync(order);
        await _dbContext.SaveChangesAsync();
        return Ok(order);
    }

    // Update Order
    // Upon updating an order, the order status is changed, and items are added or removed.
    // Items added or removed will not be reflected in the order's total price, if the order has already been paid for.
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] PutOrderRequest request)
    {
        var order = await _dbContext.Orders.FindAsync(id);
        if (order == null)
        {
            return NotFound("Order not found");
        }
        
        await _dbContext.SaveChangesAsync();
        return Ok(order);
    }

    // Delete Order
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var order = await _dbContext.Orders.FindAsync(id);
        if (order == null)
        {
            return NotFound("Order not found");
        }
        _dbContext.Orders.Remove(order);
        await _dbContext.SaveChangesAsync();
        return Ok("Order deleted");
    }


    public OrderController(IMapper mapper, ShopDbContext dbContext, IPaginationService paginationService, IRabbitMQProducer rabbitMqProducer) : base(mapper, dbContext, paginationService, rabbitMqProducer)
    {
    }
}