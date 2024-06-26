﻿using main_service.Authentication;
using main_service.Authentication.Policies;
using main_service.Models;
using main_service.RabbitMQ;
using main_service.Services;
using Microsoft.AspNetCore.Authorization;

namespace main_service.Configurations;

/// <summary>
/// This class will be used to configure the services for the application
///  - AddScoped: The service will be created once per request
///  - AddSingleton: The service will be created once and shared
/// </summary>
public class ServiceConfig
{
    public static void Configure(IServiceCollection services)
    {
        // Authorization handlers
        AuthorizationHandlers(services);
        // Services
        ServiceClasses(services);
    }


    /// <summary>
    /// This will be used to add services classes, such as the BlobService, EmailService etc.
    /// </summary>
    private static void ServiceClasses(IServiceCollection services)
    {
        // Services
        services.AddScoped<IBlobService, BlobService>();
        services.AddAutoMapper(typeof(MappingProfile));
        services.AddSingleton<IPaginationService, PaginationService>();
        services.AddScoped<IOrderService, OrderService>();
        
        // RabbitMQ
        services.AddScoped<IRabbitMQProducer, RabbitMQProducer>();
        // JwtHelper
        services.AddSingleton<JwtHelper>();
    }

    /// <summary>
    /// This adds the authorizations which checks if the user is an admin or a user
    /// </summary>
    private static void AuthorizationHandlers(IServiceCollection services)
    {
        services.AddSingleton<IAuthorizationHandler, IsUserHandler>();
        services.AddSingleton<IAuthorizationHandler, IsAdminHandler>();
    }
}