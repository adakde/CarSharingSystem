using CarSharingSystem.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace CarSharingSystem.Models.Entities
{
    public class Car
    {
        public Guid Id { get; set; }
        [Required]
        public string Brand { get; set; }
        [Required]
        public string Model { get; set; }
        [Required]
        public int YearOfProduction { get; set; }
        [Required]
        public string? Body { get; set; }
        public double Range { get; set; }
        public double LoadingTime { get; set; }
        public Status Status { get; set; }
    }
}
