using main_service.Models;
using Microsoft.AspNetCore.Authorization;

namespace main_service.Controllers;


/// <summary>
/// This will be the base controller for all admin controllers
/// </summary>
[Authorize(Policy = "IsAdmin")]
public class BaseAdminController : BaseController
{
    // If we utilize AutoMapper, we can inject it here
    // protected readonly IMapper Mapper;
    protected readonly ShopDbContext DbContext;

    public BaseAdminController(ShopDbContext dbContext)
    {
        DbContext = dbContext;
    }
}