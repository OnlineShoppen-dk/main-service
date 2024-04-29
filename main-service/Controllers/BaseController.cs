using AutoMapper;
using main_service.Models;
using main_service.RabbitMQ;
using main_service.Services;
using Microsoft.AspNetCore.Mvc;

namespace main_service.Controllers;

public class BaseController : ControllerBase
{
    protected readonly IMapper _mapper;
    protected readonly ShopDbContext _dbContext;
    protected readonly IPaginationService _paginationService;
    protected readonly IRabbitMQProducer _rabbitMqProducer;

    public BaseController(IMapper mapper, ShopDbContext dbContext, IPaginationService paginationService, IRabbitMQProducer rabbitMqProducer)
    {
        _mapper = mapper;
        _dbContext = dbContext;
        _paginationService = paginationService;
        _rabbitMqProducer = rabbitMqProducer;
    }
    
}