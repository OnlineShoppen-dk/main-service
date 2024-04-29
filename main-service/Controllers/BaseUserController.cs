using AutoMapper;
using main_service.Authentication;
using main_service.Models;
using main_service.RabbitMQ;
using main_service.Services;
using Microsoft.AspNetCore.Authorization;

namespace main_service.Controllers;


/// <summary>
/// This will be the base controller for all user controllers
/// So any controller that needs to be accessed by a user will inherit from this class
/// </summary>
[Authorize(Policy = "IsUser")]
public class BaseUserController : BaseController
{
    protected UserPrincipal UserPrincipal
    {
        get
        {
            var currentlyLoggedUser = JwtHelper.GetUser(this.User);
            return currentlyLoggedUser;
        }
    }

    public BaseUserController(IMapper mapper, ShopDbContext dbContext, IPaginationService paginationService, IRabbitMQProducer rabbitMqProducer) : base(mapper, dbContext, paginationService, rabbitMqProducer)
    {
    }
}