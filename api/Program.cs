
using FCamara.CommissionCalculator.Services;
using Microsoft.Extensions.Logging;

namespace FCamara.CommissionCalculator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure logging
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
            builder.Logging.AddFile("Logs/daily_log_{Date}.txt");

            // Add services to the container.
            builder.Services.AddScoped<ICommissionService, CommissionService>();
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            var logger = app.Services.GetRequiredService<ILogger<Program>>();
            logger.LogInformation("Application started successfully at {Time}", DateTime.Now);

            app.MapControllers();

            app.Run();
        }
    }
}
