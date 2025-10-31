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
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userId = CurrentUserId();

            var car = await _context.Cars.FindAsync(dto.CarId);
            if (car == null) return NotFound("Car not found");
            if (car.Status != CarStatus.Available) return Conflict("Car is not available");

            // Sprawdzanie kolizji terminów
            var overlap = await _context.Rentals.AnyAsync(r =>
                r.CarId == dto.CarId &&
                r.Status == RentalStatus.Active &&
                r.EndRental > dto.StartRental && dto.EndRental > r.StartRental);

            if (overlap) return Conflict("Overlapping reservation");
            var rental = new Rental
            {
                RentalId = Guid.NewGuid(),
                UserId = userId,
                CarId = car.CarId,
                StartRental = dto.StartRental.ToUniversalTime(),
                EndRental = dto.EndRental.ToUniversalTime(),
                Status = RentalStatus.Active,
                MethodOfPayment = dto.MethodOfPayment,
                Description = dto.Description ?? string.Empty,
                IsPaid = false
            };

            car.Status = CarStatus.Borrowed;

            _context.Rentals.Add(rental);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Reservation created successfully", rental.RentalId });
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

    }
}
