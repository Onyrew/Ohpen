
using Ohpen.db.interfaces;
using Ohpen.services.interfaces;
using Ohpen.services;
using Ohpen.db;
using OhpenExt.processor;
using Ohpen.item;

namespace Ohpen
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddScoped<IJobService, JobService>();
            builder.Services.AddSingleton<IJobRepository, JobRepository>();
            builder.Services.AddTransient<IProcessor, MockProcessor>();
            builder.Services.AddLogging();

            builder.Host.ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();
            });
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
