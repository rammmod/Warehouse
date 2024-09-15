using WarehouseApi.App;
using WarehouseApi.Middlewares;
using System.Text.Json.Serialization;

namespace WarehouseApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers()
                .AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json")
                .Build();

            builder.Services.AddServices(configuration);

            var app = builder.Build();

            app.UseStatusCodePages();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}