using CarSharingSystem.Models.Enums;

namespace CarSharingSystem.Models.Entities
{
    public class Rental
    {
        public Guid RentalId { get; set; }
        public Guid? UserId { get; set; }
        public Guid? CarId { get; set; }
        public User? User { get; set; }
        public Car? Car { get; set; }
        public DateTime StartRental { get; set; }
        public DateTime EndRental { get;  set; }
        public RentalStatus Status { get; set; }
        public decimal RentalPrice { get; set; }
        public PaymentMethod MethodOfPayment { get; set; }
        public bool IsPaid { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Caution { get; set; } = string.Empty;
        public double Mileage { get; set; }
    }
}
