using Microsoft.OpenApi.Models;
using Projectx.Contracts.Logging;
using Projectx.Contracts.Repository;
using Projectx.Logger;
using Projectx.Repository.RepositoryManager;

namespace ProjectxAPI.DependencyExtenssion;
public static class DependencyExtenssion
{
    public static void ConfigureCors(this IServiceCollection services) =>
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
        });

    //Provides logger service
    public static void ConfigureLoggerService(this IServiceCollection services) =>
        services.AddTransient<ILoggerManager, LoggerManager>();

    public static void ConfigureRepositoryManager(this IServiceCollection services, IConfiguration configuration) =>
        services.AddScoped<IRepositoryManager, RepositoryManager>((s) =>
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection");
            var repostioryManager = new RepositoryManager(connectionString);
            return repostioryManager;
        });

    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(s =>
        {
            s.SwaggerDoc("v1", new OpenApiInfo { Title = "Company Employee API", Version = "v1" });

            s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Place to add JWT with Bearer",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            s.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Name = "Bearer",
                    },
                    new List<string>()
                }
            });
        });
    }
}