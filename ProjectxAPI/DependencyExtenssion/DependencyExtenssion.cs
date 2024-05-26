using Microsoft.OpenApi.Models;
using Projectx.Contracts.Logging;
using Projectx.Contracts.Repository;
using Projectx.Logger;
using Projectx.Repository.RepositoryManager;
using ProjectxAPI.Streaming;
using System.Reflection;

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

    public static void ConfigureStreamer(this IServiceCollection services, IConfiguration configuration) =>
        services.AddSingleton<IStreamer, Streamer>((s) =>
        {
            string[] steramerEndpoint = configuration.GetSection("StreamerEndpoint").Value.Split(':');
            string streamIp = steramerEndpoint[0];
            int streamPort = int.Parse(steramerEndpoint[1]);

            var streamer = new Streamer();
            streamer.Start(streamIp, streamPort);

            return streamer;
        });

    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                    Version = "v1",
                    Title = "ToDo API",
                    Description = "A simple example ASP.NET Core Web API",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Shayne Boyer",
                        Email = string.Empty,
                        Url = new Uri("https://twitter.com/spboyer"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under LICX",
                        Url = new Uri("https://example.com/license"),
                    }
            });
                // Set the comments path for the Swagger JSON and UI.
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);   
        });
    }
}
