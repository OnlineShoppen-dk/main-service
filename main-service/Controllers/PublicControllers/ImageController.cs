using AutoMapper;
using main_service.Models;
using main_service.RabbitMQ;
using main_service.Services;
using Microsoft.AspNetCore.Mvc;

namespace main_service.Controllers.PublicControllers;

[ApiController]
[Route("[controller]")]
public class ImageController : BaseController
{
    private readonly IBlobService _blobService;

    [HttpGet]
    [Route("{fileName}")]
    public async Task<ActionResult> GetImage(string fileName)
    {
        var image = await _blobService.GetImage(fileName);
        return File(image, "image/jpeg");
    }


    public ImageController(IMapper mapper, ShopDbContext dbContext, IPaginationService paginationService, IRabbitMQProducer rabbitMqProducer, IBlobService blobService) : base(mapper, dbContext, paginationService, rabbitMqProducer)
    {
        _blobService = blobService;
    }
}