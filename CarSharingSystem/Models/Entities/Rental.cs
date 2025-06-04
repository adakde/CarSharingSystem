using CarSharingSystem.Models.Enums;

namespace CarSharingSystem.Models.Entities
{
    public class Rental
    {
        public Guid Id { get; set; }
        public User User { get; set; }
        public Car Car { get; set; }
        public DateTime StartRental { get; set; }
        public DateTime EndRental { get; private set; }
        public RentalStatus Status { get; set; }
        public double RentalPrice { get; set; }
        public PaymentMethod MethodOfPayment { get; set; }
        public bool IsPaid { get; set; }
        public string Description { get; set; }
        public string Caution { get; set; }
        public double Mileage { get; set; }
    }
}
