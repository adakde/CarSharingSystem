namespace CarSharingSystem.DTOs
{
    public class ReservationResponseDto
    {
        public Guid RentalId { get; set; }
        public string Brand { get; set; } = "";
        public string Model { get; set; } = "";
        public DateTime StartRental { get; set; }
        public DateTime EndRental { get; set; }
        public string Status { get; set; } = "";
        public decimal RentalPrice { get; set; }
        public string MethodOfPayment { get; set; } = "";
    }

}
