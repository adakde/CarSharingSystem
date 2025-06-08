using CarSharingSystem.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace CarSharingSystem.DTOs
{
    public class CarUpdateDto
    {
        [StringLength(50, MinimumLength = 2)]
        public string? Brand { get; set; }

        [StringLength(50, MinimumLength = 1)]
        public string? Model { get; set; }

        [Range(1950, 2026)]
        public int? YearOfProduction { get; set; }

        public CarType? CarType { get; set; }

        [Range(0, 100)]
        public double? Battery { get; set; }

        [Range(0, 1000)]
        public int? Range { get; set; }

        [Range(0, 24)]
        public double? LoadingTime { get; set; }

        public CarStatus? Status { get; set; }

        [Range(0.01, 10000)]
        public decimal? PricePerDay { get; set; }

        public string? Location { get; set; }
    }
}
