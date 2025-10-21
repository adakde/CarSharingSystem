using CarSharingSystem.Models.Entities;

namespace CarSharingSystem.Services
{
    public class PaymentService
    {
        public decimal CalculateRentalCost(Car car, DateTime startUtc, DateTime endUtc)
        {
            if (endUtc <= startUtc) return 0m;
            var days = (decimal)Math.Ceiling((endUtc - startUtc).TotalDays);
            return days + car.PricePerDay;
        }
    }
}
