using Projectx.Contracts.Logging;
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

    public static void ConfigureRepositoryManager(this IServiceCollection services) =>
        services.AddScoped<RepositoryManager, RepositoryManager>();
}