using Microsoft.OpenApi.Models;
using Projectx.Contracts.Logging;
using Projectx.Contracts.Repository;
using Projectx.Logger;
using Projectx.Repository.RepositoryManager;
using ProjectxAPI.Streaming;

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
}