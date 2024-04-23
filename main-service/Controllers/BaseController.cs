using AutoMapper;
using main_service.Models;
using Microsoft.AspNetCore.Mvc;

namespace main_service.Controllers;

public class BaseController : ControllerBase
{
    protected readonly IMapper _mapper;
    protected readonly ShopDbContext _dbContext;

    public BaseController(IMapper mapper, ShopDbContext dbContext)
    {
        _mapper = mapper;
        _dbContext = dbContext;
    }
    
}