namespace WebApplication4
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Added HotelRepository as a scoped service to the dependency injection container
            //AddScoped means that a new instance of HotelRepository will be created
            //for each HTTP request and shared within that request.

            // AddSingleton would create a single instance for the entire application,
            // while AddTransient would create a new instance every time it's requested.
            builder.Services.AddScoped<Data.HotelRepository>(); 

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
