using CarSharingSystem.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace CarSharingSystem.DTOs
{
    public class CarCreateDto
    {
        [Required(ErrorMessage = "Brand is required")]
        [StringLength(50, MinimumLength = 2)]
        public string? Brand { get; set; }

        [Required(ErrorMessage = "Model is required")]
        [StringLength(50, MinimumLength = 1)]
        public string? Model { get; set; }

        [Range(1900, 2100, ErrorMessage = "Invalid year")]
        public int YearOfProduction { get; set; }

        [Required]
        public CarType CarType { get; set; }

        [Range(0, 100, ErrorMessage = "Battery must be between 0 and 100")]
        public double? Battery { get; set; }

        [Range(0, 1000, ErrorMessage = "Range must be positive")]
        public int? Range { get; set; }

        [Range(0, 24, ErrorMessage = "Invalid loading time")]
        public double? LoadingTime { get; set; }

        [Required]
        public CarStatus Status { get; set; }

        [Range(0.01, 10000, ErrorMessage = "Invalid price")]
        public decimal PricePerDay { get; set; }

        [Required]
        public string? Location { get; set; }
    }
}
