using CarSharingSystem.Models.Enums;

namespace CarSharingSystem.Models.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; } // Right now without hashing password
        public int NumberOfDriverLicense { get; set; }
        public string CountryOfDriverLicense { get; set; } = string.Empty;
        public string PhoneNumber { get; set; }
        public Roles Role {  get; set; }
        public string HistoryOfRental { get; set; }
    }
}
