namespace CarSharingSystem.Client.Models
{
    public class RentalDto
    {
        public Guid RentalId { get; set; }
        public string User { get; set; } = string.Empty;
        public string Car { get; set; } = string.Empty;
        public DateTime StartRental { get; set; }
        public DateTime EndRental { get; set; }
        public string Status { get; set; } = string.Empty;
        public string MethodOfPayment { get; set; } = string.Empty;
        public decimal RentalPrice { get; set; }
    }
}
