
using Microsoft.EntityFrameworkCore;
using WebApiDemo.Data;
using WebApiDemo.Filters;
using WebApiDemo.Services;
using WebApiDemo.Services.Interfaces;

namespace WebApiDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Logging.AddLog4Net("CfgFile/log4net.config");

            builder.Services.AddDbContext<AppDbContext>(option => 
            {
                option.UseNpgsql(builder.Configuration.GetConnectionString("Default")!);
            });

            builder.Services.AddTransient<IBaseService, BaseService>();
            builder.Services.AddTransient<IDeviceService, DeviceService>();

            builder.Services.AddControllers(options => 
            {
                options.Filters.Add<ActionLogFilter>();
            });
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();

                using var scope = app.Services.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.EnsureCreated();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
