using System.IO.Hashing;
using System.Text.Json.Serialization;
using main_service.Configurations;

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
        // Gets the necessary configuration values
        // TODO: This needs to be done, so that the configuration values are not hardcoded
        var issuer = "OnlineShoppen.dk"; // Configuration["Jwt:Issuer"];
        var audience = "OnlineShoppen.dk"; // Configuration["Jwt:Audience"];
        var key = "SZBNheG6DYChL2oyIo6Q3dAiK4sREZGPX6orWfH2Mk="; // Configuration["Jwt:Key"];
        var connectionString = "server=localhost;user=Root;password=Password;database=mainservicedb"; // Configuration.GetConnectionString("conn");

        /* Checks if any of the configuration values are missing
        if (issuer is null || audience is null || key is null || connectionString is null)
        {
            throw new Exception("Missing configuration");
        }
        */

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
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseCors("AllowAll");
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        app.UseAuthorization();
        app.UseAuthentication();
    }
}