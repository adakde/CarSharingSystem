using CarSharingSystem.Data;
using CarSharingSystem.DTOs;
using CarSharingSystem.Models.Entities;
using CarSharingSystem.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarSharingSystem.Controllers
{
    [Route("Api/[controller]")]
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
        [HttpPut]
        [Route("UpdateById")]
        public async Task<IActionResult> UpdateById(Guid id, [FromBody] CarUpdateDto model)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound("Car not found");
            }

            // Update only provided fields
            if (model.Brand != null) car.Brand = model.Brand;
            if (model.Model != null) car.Model = model.Model;
            if (model.YearOfProduction.HasValue) car.YearOfProduction = model.YearOfProduction.Value;
            if (model.CarType.HasValue) car.CarType = model.CarType.Value;
            if (model.Battery.HasValue) car.Battery = (decimal)model.Battery.Value;
            if (model.Range.HasValue) car.Range = model.Range.Value;
            if (model.LoadingTime.HasValue) car.LoadingTime = model.LoadingTime.Value;
            if (model.Status.HasValue) car.Status = model.Status.Value;
            if (model.PricePerDay.HasValue) car.PricePerDay = model.PricePerDay.Value;
            if (model.Location != null) car.Location = model.Location;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(car); // Return the updated car object
                                // Or if you have a DTO: return Ok(_mapper.Map<CarDto>(car));
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, "Error updating car");
            }
        }
        [HttpPost]
        [Route("AddCar")]
        public async Task<IActionResult> AddCar([FromBody] Car newCar)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            newCar.CarId = Guid.NewGuid();

            _context.Cars.Add(newCar);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = newCar.CarId }, newCar);
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
