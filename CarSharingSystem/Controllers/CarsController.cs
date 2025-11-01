using CarSharingSystem.Data;
using CarSharingSystem.DTOs;
using CarSharingSystem.Models.Entities;
using CarSharingSystem.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarSharingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : Controller
    {
        private readonly CarSharingContext _context;
        public CarsController(CarSharingContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAllCars()
        {
            var Cars = await _context.Cars.ToListAsync();
            if (Cars == null || Cars.Count == 0) {
                return NotFound("No cars found");
            }
            return Ok(Cars);
        }
        [HttpGet]
        [Route("GetByBrand")]
        public async Task<IActionResult> GetByBrand(string brand)
        {
            var cars = await _context.Cars
                                     .Where(c => c.Brand.ToLower() == brand.ToLower())
                                     .ToListAsync();
            if (!cars.Any())
            {
                return NotFound($"No cars found for brand {brand}");
            }
            return Ok(cars);
        }
        [HttpGet]
        [Route("GetByType")]
        public async Task<IActionResult> GetByType(string type)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                return BadRequest("Type parameter is required");
            }

            if (!Enum.TryParse(type, true, out CarType carType))
            {
                return BadRequest("Invalid car type");
            }

            var cars = await _context.Cars
                .Where(c => c.CarType == carType)
                .ToListAsync();

            return Ok(cars);
        }
        [HttpGet]
        [Route("Available")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAvailableCars()
        {
            var cars = await _context.Cars.Where(c => c.Status == CarStatus.Available).ToListAsync();
            if (!cars.Any())
            {
                return NotFound("No found available cars");
            }
            return Ok(cars);
        }

        [HttpGet]
        [Route("GetById")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound("Not found car id");
            }
            return Ok(car);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("update/{id:guid}")]
        public async Task<IActionResult> UpdateCar(Guid id, [FromBody] CarUpdateDto model)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null)
                return NotFound("Car not found");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!string.IsNullOrEmpty(model.Brand)) car.Brand = model.Brand;
            if (!string.IsNullOrEmpty(model.Model)) car.Model = model.Model;
            if (model.YearOfProduction.HasValue) car.YearOfProduction = model.YearOfProduction.Value;
            if (model.CarType.HasValue) car.CarType = model.CarType.Value;
            if (model.Battery.HasValue) car.Battery = (decimal)model.Battery.Value;
            if (model.Range.HasValue) car.Range = model.Range.Value;
            if (model.LoadingTime.HasValue) car.LoadingTime = model.LoadingTime.Value;
            if (model.Status.HasValue) car.Status = model.Status.Value;
            if (model.PricePerDay.HasValue) car.PricePerDay = model.PricePerDay.Value;
            if (!string.IsNullOrEmpty(model.Location)) car.Location = model.Location;

            await _context.SaveChangesAsync();
            return Ok(new { message = "✅ Car updated successfully!" });
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("add")]
        public async Task<IActionResult> AddCar([FromBody] CarCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join("; ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                return BadRequest($"Niepoprawne dane: {errors}");
            }

            var car = new Car
            {
                CarId = Guid.NewGuid(),
                Brand = dto.Brand,
                Model = dto.Model,
                YearOfProduction = dto.YearOfProduction,
                CarType = dto.CarType,
                Battery = (decimal?)dto.Battery ?? 0,
                Range = dto.Range ?? 0,
                LoadingTime = dto.LoadingTime ?? 0,
                Status = dto.Status,
                PricePerDay = dto.PricePerDay,
                Location = dto.Location
            };

            _context.Cars.Add(car);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Samochód dodany pomyślnie!" });
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteCar(Guid id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null) return NotFound("Samochód nie istnieje.");

            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Samochód usunięty." });
        }

        [HttpDelete]
        [Route("DeleteAll")]
        public async Task<IActionResult> DeleteAll()
        {
            var cars = await _context.Cars.ToListAsync();
            if(cars.Any())
            {
                _context.Cars.RemoveRange(cars);
                await _context.SaveChangesAsync();
                return Ok($"Deleted {cars.Count} cars");
            }
            else
            {
                return NotFound("No cars found to delete");
            }

        }

    }
}
