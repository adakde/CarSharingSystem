using CarSharingSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarSharingSystem.Controllers
{
    [ApiController]
    public class CarsController : Controller
    {
        private readonly CarSharingContext _context;
        public CarsController(CarSharingContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("Cars/GetAll")]
        public async Task<IActionResult> GetAllCars()
        {
            var Cars = await _context.Cars.ToListAsync();

            return Ok(Cars);
        }
    }
}
