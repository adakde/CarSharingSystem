using CarSharingSystem.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace CarSharingSystem.Models.Entities
{
    public class Car
    {
        public Guid CarId { get; set; }
        [Required]
        public string Brand { get; set; } = string.Empty;
        [Required]
        public string Model { get; set; } = string.Empty;
        [Required]
        public int YearOfProduction { get; set; }
        [Required]
        public CarType CarType { get; set; }
        public decimal Battery { get; set; }
        public double Range { get; set; }
        public double LoadingTime { get; set; }
        public CarStatus Status { get; set; }
        public decimal PricePerDay { get; set; }
        public string Location { get; set; } = string.Empty;
        public List<Rental>? Rentals { get; set; }
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
    }
}
