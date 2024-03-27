using Microsoft.AspNetCore.Mvc;

namespace main_service.Controllers.Tests;

[Route("[controller]")]
[ApiController]
public class TestController : BaseController
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Hello World");
    }
    
}