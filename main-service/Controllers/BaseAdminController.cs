using Microsoft.AspNetCore.Authorization;

namespace main_service.Controllers;


/// <summary>
/// This will be the base controller for all admin controllers
/// </summary>
[Authorize(Policy = "IsAdmin")]
public class BaseAdminController : BaseController
{
    
}