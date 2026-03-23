using Microsoft.EntityFrameworkCore;
using WebApplication4.ConfigurationClass;
using WebApplication4.CustomMiddlewares;
using WebApplication4.Data;

namespace WebApplication4
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            //Register the DbContext with the dependency injection container
            builder.Services.AddDbContext<HotelContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            // Register the repository with the dependency injection container
            builder.Services.AddScoped<IHotelRepository, HotelRepository>();

            builder.Services.Configure<EmailSettings>(
                builder.Configuration.GetSection(EmailSettings.SectionName));

            builder.Services.AddTransient<RequestLoggingMiddleware>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();
            // Optional Auto-migrate database on application startup (use with caution in production)
            //using(var scope = app.Services.CreateScope())
            //{
            //    var dbContext = scope.ServiceProvider.GetRequiredService<HotelContext>();
            //    dbContext.Database.Migrate();// Automatically apply any pending migrations to the database
            //}
            app.Run();
        }
    }
}
