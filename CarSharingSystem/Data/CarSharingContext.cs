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
        public DbSet<User> users { get; set; }
    }
}
