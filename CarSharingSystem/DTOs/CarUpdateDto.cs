using CarSharingSystem.Models.Enums;
using System.Text.Json.Serialization;

namespace CarSharingSystem.DTOs
{
    public class CarUpdateDto
    {
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public int? YearOfProduction { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public CarType? CarType { get; set; }

        public double? Battery { get; set; }
        public int? Range { get; set; }
        public double? LoadingTime { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public CarStatus? Status { get; set; }

        public decimal? PricePerDay { get; set; }
        public string? Location { get; set; }
    }
}
