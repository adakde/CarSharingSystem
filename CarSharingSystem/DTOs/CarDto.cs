using CarSharingSystem.Models.Enums;

namespace CarSharingSystem.DTOs
{
    public class CarDto
    {
        public Guid CarId { get; set; }
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int YearOfProduction { get; set; }
        public CarType CarType { get; set; }
        public double? Battery { get; set; } 
        public int? Range { get; set; } 
        public double? LoadingTime { get; set; }
        public CarStatus Status { get; set; }
        public decimal PricePerDay { get; set; }
        public string Location { get; set; } = string.Empty;
    }
}
