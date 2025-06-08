using CarSharingSystem.Models.Enums;

namespace CarSharingSystem.DTOs
{
    public class CarUpdateDto
    {
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int YearOfProduction { get; set; }
        public CarType CarType { get; set; }
        public decimal Battery { get; set; }
        public double Range { get; set; }
        public double LoadingTime { get; set; }
        public CarStatus Status { get; set; }
        public decimal PricePerDay { get; set; }
        public string Location { get; set; } = string.Empty;
    }
}
