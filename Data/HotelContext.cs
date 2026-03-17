using Microsoft.EntityFrameworkCore;

namespace WebApplication4.Data
{
    public class HotelContext : DbContext
    {
        public HotelContext(DbContextOptions<HotelContext> options) : base(options)
        {

        }
        public DbSet<Hotel> Hotels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Fluent API configuration for Hotel entity(alternative to data annotations)
            modelBuilder.Entity<Hotel>()
                .Property(p=>p.HotelName)
                .IsRequired()
                .HasMaxLength(100);
            // Seed data
            modelBuilder.Entity<Hotel>().HasData(
                new Hotel
                {
                    Id = 1,
                    HotelName = "Hotel A",
                    Rating = 4,
                    Address = "123 Main St",
                    EmailAddress = "pkaur@gmail.com"
                   // CreatedDate = DateTime.Now
                },
                new Hotel
                {
                    Id = 2,
                    HotelName = "Hotel B",
                    Rating = 5,
                    Address = "456 Main St",
                    EmailAddress = "agam@gmail.com"
                  //  CreatedDate = DateTime.Now

                });


                }
    }
}
