using CarSharingSystem.Models.Entities;
using CarSharingSystem.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace CarSharingSystem.Data
{
    public static class DataSeeder
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            SeedUsers(modelBuilder);
            SeedCars(modelBuilder);
            SeedRentals(modelBuilder);
        }
        private static void SeedUsers(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserId = Guid.NewGuid(),
                    Email = "admin@example.com",
                    Password = "Admin123!", // Hashowanie hasła
                    Name = "Admin",
                    Role = UserRole.Admin
                },
                new User
                {
                    UserId = Guid.NewGuid(),
                    Email = "user1@example.com",
                    Password = "User123!",
                    Name = "Jan Kowalski",
                    Role = UserRole.User
                }
            );
        }
        private static void SeedCars(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Car>().HasData(
                new Car
                {
                    CarId = Guid.NewGuid(),
                    Brand = "Tesla",
                    Model = "Model 3",
                    YearOfProduction = 2022,
                    Range = 450, // km
                    LoadingTime = 8, // godziny
                    CarType = CarType.Sedan,
                    Status = CarStatus.Available,
                    PricePerDay = 300
                },
                new Car
                {
                    CarId = Guid.NewGuid(),
                    Brand = "Tesla",
                    Model = "Model X",
                    YearOfProduction = 2021,
                    Range = 270,
                    LoadingTime = 6,
                    CarType = CarType.Hatchback,
                    Status = CarStatus.Available,
                    PricePerDay = 200
                }
            );
        }
        private static void SeedRentals(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Rental>().HasData(
                new Rental
                {
                    RentalId = Guid.NewGuid(),
                    StartRental = DateTime.Now.AddDays(-1),
                    EndRental = DateTime.Now.AddDays(2),
                    Status = RentalStatus.Active,
                    MethodOfPayment = PaymentMethod.CreditCard,
                    RentalPrice = 900 
                }
            );
        }
    }
}