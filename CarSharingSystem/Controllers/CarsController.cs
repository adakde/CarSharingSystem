using CarSharingSystem.Data;
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
        public async Task<IActionResult> UpdateById(Guid id, Car model)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound("Not found car for update");
            }
            else
            {
                car.Brand = model.Brand;
                car.Status = model.Status;
                car.Battery = model.Battery;
                car.PricePerDay = model.PricePerDay;
                car.CarType = model.CarType;
                car.LoadingTime = model.LoadingTime;
                car.Model = model.Model;
                car.Range = model.Range;
                car.YearOfProduction = model.YearOfProduction;
                car.Rentals = model.Rentals;
                car.Location = model.Location;
                _context.SaveChanges();
                return RedirectToAction("Read");
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
