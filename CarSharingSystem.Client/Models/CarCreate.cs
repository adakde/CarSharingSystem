using CarSharingSystem.Client.Models;

namespace CarSharingSystem.Client.Models
{
    public class CarCreate
    {
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int YearOfProduction { get; set; } = DateTime.Now.Year;
        public CarType CarType { get; set; }
        public double? Battery { get; set; }
        public int? Range { get; set; }
        public double? LoadingTime { get; set; }
        public CarStatus Status { get; set; } = CarStatus.Available;
        public decimal PricePerDay { get; set; }
        public string Location { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }

    }
}
