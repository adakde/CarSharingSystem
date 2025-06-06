using Microsoft.SqlServer;
using CarSharingSystem.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarSharingSystem.Data
{
    public class CarSharingContext : DbContext
    {
        public CarSharingContext(DbContextOptions<CarSharingContext> options)
    : base(options)
        {
        }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Rental> Rental { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Car>()
                .Property(c => c.Battery)
                .HasPrecision(18, 2);
            modelBuilder.Entity<Car>()
                .Property(c => c.PricePerDay)
                .HasPrecision(18, 2);
            modelBuilder.Entity<Rental>()
                .Property(c => c.RentalPrice)
                .HasPrecision(18, 2);
        }
    }
}
