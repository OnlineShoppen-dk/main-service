using AutoMapper;
using main_service.Models;
using main_service.Models.ApiModels.UserDetailApiModels;
using main_service.Models.DomainModels;
using main_service.Models.DtoModels;
using main_service.RabbitMQ;
using main_service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace main_service.Controllers.UserControllers;

/// <summary>
/// Includes all user-related actions
/// Such as:
/// - Get user details
/// - Update user details
/// </summary>
[ApiController]
[Route("api/user/[controller]")]
public class UserController : BaseUserController
{
    // Anonymous user actions
    [HttpPost]
    [Route("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterNewUserRequest request)
    {
        var userExists = await _dbContext.UserDetails.AnyAsync(x => x.Email == request.Email);
        if (userExists)
        {
            return Conflict("User already exists");
        }
        var user = new UserDetails
        {
            Guid = request.Guid,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber ?? "",
        };
        await _dbContext.UserDetails.AddAsync(user);
        await _dbContext.SaveChangesAsync();
        return Ok(user);
    }
    
    // Authenticated user actions
    [HttpGet]
    [Route("get-details")]
    public async Task<IActionResult> GetDetails()
    {
        var user = await _dbContext.UserDetails.FindAsync(UserPrincipal.Id);
        if (user == null)
        {
            return NotFound("User not found");
        }
        var response = _mapper.Map<UserDetailsDto>(user);
        return Ok(response);
    }
    
    [HttpPut]
    [Route("update-details")]
    public async Task<IActionResult> UpdateDetails([FromBody] PutUserDetailRequest request)
    {
        var user = await _dbContext.UserDetails.FindAsync(UserPrincipal.Id);
        if (user == null)
        {
            return NotFound("User not found");
        }
        user.FirstName = request.FirstName ?? user.FirstName;
        user.LastName = request.LastName ?? user.LastName;
        user.Email = request.Email ?? user.Email;
        user.PhoneNumber = request.PhoneNumber ?? user.PhoneNumber;
        await _dbContext.SaveChangesAsync();
        var response = _mapper.Map<UserDetailsDto>(user);
        return Ok(response);
    }
    
    [HttpGet]
    [Route("get-orders")]
    public async Task<IActionResult> GetOrders()
    {
        var user = await _dbContext.UserDetails
            .Include(x => x.Orders)
            .ThenInclude(x => x.OrderItems)
            .ThenInclude(x => x.Product)
            .FirstOrDefaultAsync(x => x.Id == UserPrincipal.Id);
        if (user == null)
        {
            return NotFound("User not found");
        }
        var response = _mapper.Map<List<OrderDto>>(user.Orders);
        return Ok(response);
    }
    
    public UserController(IMapper mapper, ShopDbContext dbContext, IPaginationService paginationService, IRabbitMQProducer rabbitMqProducer) : base(mapper, dbContext, paginationService, rabbitMqProducer)
    {
    }
}