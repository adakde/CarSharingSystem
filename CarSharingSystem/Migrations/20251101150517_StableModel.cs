using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CarSharingSystem.Migrations
{
    /// <inheritdoc />
    public partial class StableModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cars",
                columns: table => new
                {
                    CarId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    YearOfProduction = table.Column<int>(type: "int", nullable: false),
                    CarType = table.Column<int>(type: "int", nullable: false),
                    Battery = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Range = table.Column<double>(type: "float", nullable: false),
                    LoadingTime = table.Column<double>(type: "float", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    PricePerDay = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cars", x => x.CarId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumberOfDriverLicense = table.Column<int>(type: "int", nullable: false),
                    CountryOfDriverLicense = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<int>(type: "int", nullable: false),
                    HistoryOfRental = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Rentals",
                columns: table => new
                {
                    RentalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CarId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    StartRental = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndRental = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    RentalPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    MethodOfPayment = table.Column<int>(type: "int", nullable: false),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Caution = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mileage = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rentals", x => x.RentalId);
                    table.ForeignKey(
                        name: "FK_Rentals_Cars_CarId",
                        column: x => x.CarId,
                        principalTable: "Cars",
                        principalColumn: "CarId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rentals_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Cars",
                columns: new[] { "CarId", "Battery", "Brand", "CarType", "Description", "ImageUrl", "LoadingTime", "Location", "Model", "PricePerDay", "Range", "Status", "YearOfProduction" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000003"), 100.00m, "Tesla", 0, "", "", 8.0, "", "Model 3", 300m, 450.0, 0, 2022 },
                    { new Guid("00000000-0000-0000-0000-000000000004"), 85.50m, "Tesla", 1, "Seed car Tesla Model 3", "", 6.0, "", "Model X", 200m, 270.0, 0, 2021 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "CountryOfDriverLicense", "Email", "HistoryOfRental", "Name", "NumberOfDriverLicense", "Password", "PhoneNumber", "Role" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000001"), "", "admin@example.com", null, "Admin", 0, "$2a$11$qvLZVxw81c5h6YzA5P4JtuB4AgjOmFKWk96Kk4y7xZ0zH3vILsb.q", null, 2 },
                    { new Guid("00000000-0000-0000-0000-000000000002"), "", "user1@2example.com", null, "Jan Kowalski", 0, "$2a$11$Z8iXshfUZ8Yv1q1O7q4aDeP67AWhEbkGcZzZbLwYxI6w4bA3qneU6", null, 0 }
                });

            migrationBuilder.InsertData(
                table: "Rentals",
                columns: new[] { "RentalId", "CarId", "Caution", "Description", "EndRental", "IsPaid", "MethodOfPayment", "Mileage", "RentalPrice", "StartRental", "Status", "UserId" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000005"), new Guid("00000000-0000-0000-0000-000000000003"), "", "", new DateTime(2024, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 1, 0.0, 900m, new DateTime(2024, 6, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, new Guid("00000000-0000-0000-0000-000000000002") });

            migrationBuilder.CreateIndex(
                name: "IX_Rentals_CarId",
                table: "Rentals",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_Rentals_UserId",
                table: "Rentals",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rentals");

            migrationBuilder.DropTable(
                name: "Cars");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
