using CarSharingSystem.Data;
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

    }
}
