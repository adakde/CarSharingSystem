using CarSharingSystem.Models.Enums;

namespace CarSharingSystem.DTOs
{
    public class CarFilterDto
    {
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public CarType? CarType { get; set; }
        public CarStatus? Status { get; set; }
        public decimal? MaxPricePerDay { get; set; }
        public int? MinRange { get; set; }
    }
}
