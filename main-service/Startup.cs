using main_service.Configurations;
using main_service.Models;
using Microsoft.EntityFrameworkCore;

namespace main_service;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }
    
    private IConfiguration Configuration { get; }
    
    public void ConfigureServices(IServiceCollection services)
    {
        /* Gets the necessary configuration values, if run locally gets local values, README.md for setting those values */
        var issuer = Environment.GetEnvironmentVariable("ISSUER") ?? Configuration["Jwt:Issuer"];
        var audience = Environment.GetEnvironmentVariable("AUDIENCE") ?? Configuration["Jwt:Audience"];
        var key = Environment.GetEnvironmentVariable("KEY") ?? Configuration["Jwt:Key"];
        var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING") ?? Configuration.GetConnectionString("conn");

        if (issuer is null || audience is null || key is null || connectionString is null)
        {
            throw new Exception("Missing configuration");
        }

        AuthenticationConfig.Configure(services, issuer, audience, key);
        AuthorizationConfig.Configure(services);
        ServiceConfig.Configure(services);
        DatabaseConfig.Configure(services, connectionString);
        SwaggerConfig.Configure(services);
        
        services.AddControllers();

        // Adds cors policy which allows any origin, method and header
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", builder =>
            {
                builder.WithOrigins("http://localhost:3000")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ShopDbContext dbContext)
    {
        // Migrate database to latest version if necessary
        dbContext.Database.Migrate();
        
        app.UseCors("AllowAll");
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthorization();
        app.UseAuthentication();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}