using CarSharingSystem.Models.Entities;
using CarSharingSystem.Models.Enums;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace CarSharingSystem.Data
{
    public static class DataSeeder
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            // Stałe identyfikatory dla spójności danych
            var adminId = Guid.Parse("00000000-0000-0000-0000-000000000001");
            var userId = Guid.Parse("00000000-0000-0000-0000-000000000002");
            var car1Id = Guid.Parse("00000000-0000-0000-0000-000000000003");
            var car2Id = Guid.Parse("00000000-0000-0000-0000-000000000004");
            var rentalId = Guid.Parse("00000000-0000-0000-0000-000000000005");

            SeedUsers(modelBuilder, adminId, userId);
            SeedCars(modelBuilder, car1Id, car2Id);
            SeedRentals(modelBuilder, rentalId, userId, car1Id);
        }

        private static void SeedUsers(ModelBuilder modelBuilder, Guid adminId, Guid userId)
        {
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserId = adminId,
                    Email = "admin@example.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                    Name = "Admin",
                    Role = UserRole.Admin
                },
                new User
                {
                    UserId = userId,
                    Email = "user1@example.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("User123!"),
                    Name = "Jan Kowalski",
                    Role = UserRole.User
                }
            );
        }

        private static void SeedCars(ModelBuilder modelBuilder, Guid car1Id, Guid car2Id)
        {
            modelBuilder.Entity<Car>().HasData(
                new Car
                {
                    CarId = Guid.Parse("00000000-0000-0000-0000-000000000003"),
                    Brand = "Tesla",
                    Model = "Model 3",
                    YearOfProduction = 2022,
                    Range = 450,
                    LoadingTime = 8,
                    CarType = CarType.Sedan,
                    Status = CarStatus.Available,
                    PricePerDay = 300,
                    Battery = 100.00m
                },
                new Car
                {
                    CarId = Guid.Parse("00000000-0000-0000-0000-000000000004"),
                    Brand = "Tesla",
                    Model = "Model X",
                    YearOfProduction = 2021,
                    Range = 270,
                    LoadingTime = 6,
                    CarType = CarType.Hatchback,
                    Status = CarStatus.Available,
                    PricePerDay = 200,
                    Battery = 85.50m
                }
            );

        }

        private static void SeedRentals(ModelBuilder modelBuilder, Guid rentalId, Guid userId, Guid carId)
        {
            modelBuilder.Entity<Rental>().HasData(
                new Rental
                {
                    RentalId = rentalId,
                    UserId = userId,
                    CarId = carId,
                    StartRental = new DateTime(2024, 6, 7),
                    EndRental = new DateTime(2024, 6, 10),
                    Status = RentalStatus.Active,
                    MethodOfPayment = PaymentMethod.CreditCard,
                    RentalPrice = 900
                }
            );
        }
    }
}