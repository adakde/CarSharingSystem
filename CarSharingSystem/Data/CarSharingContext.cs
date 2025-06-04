using Microsoft.SqlServer;
using CarSharingSystem.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarSharingSystem.Data
{
    public class CarSharingContext : DbContext
    {
        public DbSet<Car> Cars { get; set; }
        public DbSet<Rental> Rental { get; set; }
        public DbSet<User> users { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=CarSharingDb;Trusted_Connection=True;");
        }
    }
}
