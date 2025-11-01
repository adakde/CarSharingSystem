using CarSharingSystem.Data;
using CarSharingSystem.DTOs;
using CarSharingSystem.Models.Entities;
using CarSharingSystem.Models.Enums;
using CarSharingSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CarSharingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReservationsController : ControllerBase
    {
        private readonly CarSharingContext _context;
        private readonly PaymentService _paymentService;

        public ReservationsController(CarSharingContext context, PaymentService paymentService)
        {
            _context = context;
            _paymentService = paymentService;
        }

        private Guid CurrentUserId()
        {
            var sub = User.FindFirstValue(ClaimTypes.NameIdentifier)
                   ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (string.IsNullOrEmpty(sub))
                throw new UnauthorizedAccessException("Brak identyfikatora użytkownika w tokenie JWT.");

            return Guid.Parse(sub);
        }
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] ReservationCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = CurrentUserId();

            var car = await _context.Cars.FirstOrDefaultAsync(c => c.CarId == dto.CarId);
            if (car == null)
                return NotFound("Car not found");

            if (car.Status != CarStatus.Available)
                return Conflict("Car is not available");

            var overlap = await _context.Rentals.AnyAsync(r =>
                r.CarId == dto.CarId &&
                r.Status == RentalStatus.Active &&
                r.EndRental > dto.StartRental && dto.EndRental > r.StartRental);

            if (overlap)
                return Conflict("Overlapping reservation");

            var totalDays = (dto.EndRental.Date - dto.StartRental.Date).Days;
            if (totalDays <= 0)
                totalDays = 1;

            var totalPrice = car.PricePerDay * totalDays;

            var rental = new Rental
            {
                CarId = dto.CarId,
                UserId = userId,
                StartRental = dto.StartRental,
                EndRental = dto.EndRental,
                Status = RentalStatus.Active,
                MethodOfPayment = dto.MethodOfPayment,
                RentalPrice = totalPrice
            };

            // 🔹 Aktualizacja statusu samochodu
            car.Status = CarStatus.Borrowed;

            _context.Rentals.Add(rental);
            await _context.SaveChangesAsync();

            return Ok(new { rental.RentalId });
        }

        [HttpPost("end/{id:guid}")]
        public async Task<IActionResult> EndReservation([FromRoute] Guid id)
        {
            var rental = await _context.Rentals
                .Include(r => r.Car)
                .FirstOrDefaultAsync(r => r.RentalId == id);

            if (rental == null) return NotFound("Reservation not found");
            if (rental.Status != RentalStatus.Active) return Conflict("Reservation is not active");

            rental.EndRental = DateTime.UtcNow;
            rental.RentalPrice = _paymentService.CalculateRentalCost(rental.Car!, rental.StartRental, rental.EndRental);
            rental.Status = RentalStatus.Closed;
            rental.IsPaid = true;

            rental.Car!.Status = CarStatus.Available;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Reservation ended", price = rental.RentalPrice });
        }

        [HttpGet("my")]
        public async Task<IActionResult> MyReservations()
        {
            var userId = CurrentUserId();
            var list = await _context.Rentals
                .Include(r => r.Car)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.StartRental)
                .ToListAsync();

            return Ok(list);
        }
        [HttpGet("busy/{carId:guid}")]
        public async Task<IActionResult> GetBusyDates(Guid carId)
        {
            var reservations = await _context.Rentals
                .Where(r => r.CarId == carId && r.Status == RentalStatus.Active)
                .Select(r => new
                {
                    from = r.StartRental,
                    to = r.EndRental
                })
                .ToListAsync();

            return Ok(reservations);
        }

        [HttpGet("proforma/{rentalId:guid}")]
        public async Task<IActionResult> GetProforma(Guid rentalId)
        {
            var rental = await _context.Rentals
                .Include(r => r.Car)
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.RentalId == rentalId);

            if (rental == null)
                return NotFound("Nie znaleziono rezerwacji.");

            using var stream = new MemoryStream();
            using (var writer = new StreamWriter(stream))
            {
                writer.WriteLine("FAKTURA PROFORMA");
                writer.WriteLine("----------------------------");
                writer.WriteLine($"Data wystawienia: {DateTime.Now:yyyy-MM-dd}");
                writer.WriteLine($"Numer: {rental.RentalId}");
                writer.WriteLine();
                writer.WriteLine($"Klient: {rental.User?.Name} ({rental.User?.Email})");
                writer.WriteLine($"Samochód: {rental.Car?.Brand} {rental.Car?.Model}");
                writer.WriteLine($"Okres: {rental.StartRental:yyyy-MM-dd} → {rental.EndRental:yyyy-MM-dd}");
                writer.WriteLine($"Kwota brutto: {rental.RentalPrice} PLN");
                writer.WriteLine("----------------------------");
                writer.WriteLine("Dziękujemy za skorzystanie z CarSharingSystem!");
                writer.Flush();
            }

            var bytes = stream.ToArray();

            // Możesz w przyszłości użyć biblioteki PDF (np. iText7, QuestPDF),
            // ale na start wystarczy czysty tekst + nagłówek PDF.
            return File(bytes, "application/pdf", $"Proforma_{rental.RentalId}.pdf");
        }

        [HttpGet("history")]
        public async Task<IActionResult> History()
        {
            var userId = CurrentUserId();

            var history = await _context.Rentals
                .Include(r => r.Car)
                .Where(r => r.UserId == userId &&
                            (r.Status == RentalStatus.Closed || r.Status == RentalStatus.Cancelled))
                .OrderByDescending(r => r.EndRental)
                .Select(r => new
                {
                    r.RentalId,
                    Car = new { r.Car!.Brand, r.Car.Model },
                    r.StartRental,
                    r.EndRental,
                    Status = r.Status.ToString(),
                    r.RentalPrice,
                    MethodOfPayment = r.MethodOfPayment.ToString()
                })
                .ToListAsync();

            return Ok(history);
        }
        [Authorize(Roles = "Admin,Worker")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var rentals = await _context.Rentals
                .Include(r => r.Car)
                .Include(r => r.User)
                .OrderByDescending(r => r.StartRental)
                .Select(r => new
                {
                    r.RentalId,
                    r.StartRental,
                    r.EndRental,
                    r.Status,
                    r.RentalPrice,
                    Car = r.Car.Brand + " " + r.Car.Model,
                    User = r.User.Name + " (" + r.User.Email + ")",
                    r.MethodOfPayment
                })
                .ToListAsync();

            return Ok(rentals);
        }
        [Authorize(Roles = "Admin,Worker")]
        [HttpPut("{id:guid}/status")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateStatusDto dto)
        {
            var rental = await _context.Rentals.FindAsync(id);
            if (rental == null)
                return NotFound("Nie znaleziono rezerwacji.");

            if (!Enum.TryParse<RentalStatus>(dto.Status, true, out var newStatus))
                return BadRequest("Niepoprawny status.");

            rental.Status = newStatus;
            await _context.SaveChangesAsync();

            return Ok(new { message = $"Status zmieniony na {newStatus}." });
        }

        public class UpdateStatusDto
        {
            public string Status { get; set; } = string.Empty;
        }

    }
}
