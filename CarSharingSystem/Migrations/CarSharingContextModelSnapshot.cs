﻿// <auto-generated />
using System;
using CarSharingSystem.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CarSharingSystem.Migrations
{
    [DbContext(typeof(CarSharingContext))]
    partial class CarSharingContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CarSharingSystem.Models.Entities.Car", b =>
                {
                    b.Property<Guid>("CarId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Battery")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Brand")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CarType")
                        .HasColumnType("int");

                    b.Property<double>("LoadingTime")
                        .HasColumnType("float");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("PricePerDay")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<double>("Range")
                        .HasColumnType("float");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int>("YearOfProduction")
                        .HasColumnType("int");

                    b.HasKey("CarId");

                    b.ToTable("Cars");

                    b.HasData(
                        new
                        {
                            CarId = new Guid("00000000-0000-0000-0000-000000000003"),
                            Battery = 100.00m,
                            Brand = "Tesla",
                            CarType = 0,
                            LoadingTime = 8.0,
                            Location = "",
                            Model = "Model 3",
                            PricePerDay = 300m,
                            Range = 450.0,
                            Status = 0,
                            YearOfProduction = 2022
                        },
                        new
                        {
                            CarId = new Guid("00000000-0000-0000-0000-000000000004"),
                            Battery = 85.50m,
                            Brand = "Tesla",
                            CarType = 1,
                            LoadingTime = 6.0,
                            Location = "",
                            Model = "Model X",
                            PricePerDay = 200m,
                            Range = 270.0,
                            Status = 0,
                            YearOfProduction = 2021
                        });
                });

            modelBuilder.Entity("CarSharingSystem.Models.Entities.Rental", b =>
                {
                    b.Property<Guid>("RentalId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CarId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Caution")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("EndRental")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsPaid")
                        .HasColumnType("bit");

                    b.Property<int>("MethodOfPayment")
                        .HasColumnType("int");

                    b.Property<double>("Mileage")
                        .HasColumnType("float");

                    b.Property<decimal>("RentalPrice")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("StartRental")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("RentalId");

                    b.HasIndex("CarId");

                    b.HasIndex("UserId");

                    b.ToTable("Rentals");

                    b.HasData(
                        new
                        {
                            RentalId = new Guid("00000000-0000-0000-0000-000000000005"),
                            CarId = new Guid("00000000-0000-0000-0000-000000000003"),
                            Caution = "",
                            Description = "",
                            EndRental = new DateTime(2024, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            IsPaid = false,
                            MethodOfPayment = 1,
                            Mileage = 0.0,
                            RentalPrice = 900m,
                            StartRental = new DateTime(2024, 6, 7, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Status = 0,
                            UserId = new Guid("00000000-0000-0000-0000-000000000002")
                        });
                });

            modelBuilder.Entity("CarSharingSystem.Models.Entities.User", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CountryOfDriverLicense")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HistoryOfRental")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NumberOfDriverLicense")
                        .HasColumnType("int");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.HasKey("UserId");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            UserId = new Guid("00000000-0000-0000-0000-000000000001"),
                            CountryOfDriverLicense = "",
                            Email = "admin@example.com",
                            Name = "Admin",
                            NumberOfDriverLicense = 0,
                            Password = "Admin123!",
                            Role = 2
                        },
                        new
                        {
                            UserId = new Guid("00000000-0000-0000-0000-000000000002"),
                            CountryOfDriverLicense = "",
                            Email = "user1@example.com",
                            Name = "Jan Kowalski",
                            NumberOfDriverLicense = 0,
                            Password = "User123!",
                            Role = 0
                        });
                });

            modelBuilder.Entity("CarSharingSystem.Models.Entities.Rental", b =>
                {
                    b.HasOne("CarSharingSystem.Models.Entities.Car", "Car")
                        .WithMany("Rentals")
                        .HasForeignKey("CarId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CarSharingSystem.Models.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Car");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CarSharingSystem.Models.Entities.Car", b =>
                {
                    b.Navigation("Rentals");
                });
#pragma warning restore 612, 618
        }
    }
}
