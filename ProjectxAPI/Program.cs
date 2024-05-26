using ProjectxAPI.DependencyExtenssion;

using NLog;

namespace ProjectxAPI;

public class Program
{
    public static void Main(string[] args)
    {
        LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
        var builder = WebApplication.CreateBuilder(args);
        //store configuration file
        IConfiguration configuration = builder.Configuration;

        // Add services to the container.
        builder.Services.ConfigureCors();
        builder.Services.ConfigureLoggerService();
        builder.Services.ConfigureRepositoryManager(configuration);
        builder.Services.ConfigureStreamer(configuration);

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
