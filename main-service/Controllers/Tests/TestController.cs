using main_service.RabbitMQ;
using Microsoft.AspNetCore.Mvc;

namespace main_service.Controllers.Tests;

[Route("/")]
[ApiController]
public class TestController : BaseController
{
    
    private readonly IRabbitMQProducer _rabbitMQProducer;

    public TestController(IRabbitMQProducer rabbitMqProducer)
    {
        _rabbitMQProducer = rabbitMqProducer;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Hello World");
    }
    
    [HttpGet("test")]
    public IActionResult Test()
    {
        Console.WriteLine("sending message");
        var message = new
        {
            Message = "Hello World From Docker"
        };
        _rabbitMQProducer.PublishMessage(message);
        return Ok("Test");
    }
    
}