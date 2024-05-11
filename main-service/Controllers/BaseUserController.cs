using AutoMapper;
using main_service.Authentication;
using main_service.Models;
using main_service.Models.DomainModels;
using main_service.RabbitMQ;
using main_service.Services;

namespace main_service.Controllers;


/// <summary>
/// This will be the base controller for all user controllers
/// So any controller that needs to be accessed by a user will inherit from this class
/// </summary>
public class BaseUserController : BaseController
{
    protected readonly JwtHelper _jwtHelper;
    protected UserPrincipal UserPrincipal
    {
        get
        {
            var token = Request.Cookies["token"];
            var decodedToken = _jwtHelper.DecodeJwtToken(token);
            // Validate that a user with the given guid exists
            var user = _dbContext.UserDetails.FirstOrDefault(u => u.Guid == decodedToken);
            if (user == null)
            {
                return null;
            }
            var userDetails = new UserPrincipal
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Role = UserRoles.User
            };
            return userDetails;

        }
    }

    public BaseUserController(IMapper mapper, ShopDbContext dbContext, IPaginationService paginationService, IRabbitMQProducer rabbitMqProducer, JwtHelper jwtHelper) : base(mapper, dbContext, paginationService, rabbitMqProducer)
    {
        _jwtHelper = jwtHelper;
    }
}