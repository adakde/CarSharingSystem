using CarSharingSystem.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace CarSharingSystem.Models.Entities
{
    public class User
    {
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public required string Name { get; set; }
        [Required]
        public required string Email { get; set; }
        [Required]
        public required string Password { get; set; } // Right now without hashing password
        public int NumberOfDriverLicense { get; set; }
        public string? CountryOfDriverLicense { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        [Required]
        public UserRole Role {  get; set; }
        public string? HistoryOfRental { get; set; }
    }
}
