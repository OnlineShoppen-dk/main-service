﻿using AutoMapper;
using main_service.Models;
using main_service.RabbitMQ;
using main_service.Services;
using Microsoft.AspNetCore.Authorization;

namespace main_service.Controllers;


/// <summary>
/// This will be the base controller for all admin controllers
/// </summary>
// [Authorize(Policy = "IsAdmin")]
public class BaseAdminController : BaseController
{
    // If we utilize AutoMapper, we can inject it here
    // protected readonly IMapper Mapper;
    public BaseAdminController(IMapper mapper, ShopDbContext dbContext, IPaginationService paginationService, IRabbitMQProducer rabbitMqProducer) : base(mapper, dbContext, paginationService, rabbitMqProducer)
    {
    }
}