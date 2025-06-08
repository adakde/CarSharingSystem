using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CarSharingSystem.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Rentals",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "CarId",
                table: "Rentals",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.InsertData(
                table: "Cars",
                columns: new[] { "CarId", "Battery", "Brand", "CarType", "LoadingTime", "Location", "Model", "PricePerDay", "Range", "Status", "YearOfProduction" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000003"), 100.00m, "Tesla", 0, 8.0, "", "Model 3", 300m, 450.0, 0, 2022 },
                    { new Guid("00000000-0000-0000-0000-000000000004"), 85.50m, "Tesla", 1, 6.0, "", "Model X", 200m, 270.0, 0, 2021 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "CountryOfDriverLicense", "Email", "HistoryOfRental", "Name", "NumberOfDriverLicense", "Password", "PhoneNumber", "Role" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000001"), "", "admin@example.com", null, "Admin", 0, "Admin123!", null, 2 },
                    { new Guid("00000000-0000-0000-0000-000000000002"), "", "user1@example.com", null, "Jan Kowalski", 0, "User123!", null, 0 }
                });

            migrationBuilder.InsertData(
                table: "Rentals",
                columns: new[] { "RentalId", "CarId", "Caution", "Description", "EndRental", "IsPaid", "MethodOfPayment", "Mileage", "RentalPrice", "StartRental", "Status", "UserId" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000005"), new Guid("00000000-0000-0000-0000-000000000003"), "", "", new DateTime(2024, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 1, 0.0, 900m, new DateTime(2024, 6, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, new Guid("00000000-0000-0000-0000-000000000002") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "CarId",
                keyValue: new Guid("00000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "Rentals",
                keyColumn: "RentalId",
                keyValue: new Guid("00000000-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "CarId",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"));

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Rentals",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CarId",
                table: "Rentals",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);
        }
    }
}
