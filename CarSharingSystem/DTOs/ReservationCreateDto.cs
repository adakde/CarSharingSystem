using CarSharingSystem.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace CarSharingSystem.DTOs
{
    public class ReservationCreateDto
    {
        [Required]
        public Guid CarId { get; set; }
        [Required]
        public DateTime StartRental { get; set; }
        [Required]
        public DateTime EndRental { get; set; }
        [Required]
        public PaymentMethod MethodOfPayment { get; set; }
        public string? Description { get; set; }
    }
}
