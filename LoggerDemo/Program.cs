
using Microsoft.OpenApi.Models;
using Serilog;
namespace LoggerDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            #region Serilog Configuration
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Hour)
                .CreateLogger();

            // Use Serilog along with buit-in logging providers
            builder.Logging.AddSerilog();

            // Use this line to override built-in logging providers
            builder.Host.UseSerilog();
            #endregion

            builder.Logging.ClearProviders();

            builder.Services.AddControllers();


            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "LoggerDemo",
                    Version = "v1",
                    Description = "A simple API demo using ILogger"
                });
            });

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "LoggerDemoAPI v1");
                c.RoutePrefix = "swagger"; // Swagger available at /swagger
            });

            app.UseHttpsRedirection();

            app.MapControllers();

            app.Run();

        }
    }
}
