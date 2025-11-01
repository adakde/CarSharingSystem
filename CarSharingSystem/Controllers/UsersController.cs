using CarSharingSystem.Data;
using CarSharingSystem.DTOs;
using CarSharingSystem.Models.Entities;
using CarSharingSystem.Models.Enums;
using CarSharingSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace CarSharingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly CarSharingContext _context;
        private readonly TokenService _tokenService;

        public UsersController(CarSharingContext context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var exists = await _context.Users.AnyAsync(u => u.Email == dto.Email);
            if (exists) return Conflict("Email already registered");

            var user = new User
            {
                UserId = Guid.NewGuid(),
                Name = dto.Name,
                Email = dto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = UserRole.User
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Me), new { }, new { user.UserId, user.Email, user.Name });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null) return Unauthorized("Invalid credentials");

            var ok = BCrypt.Net.BCrypt.Verify(dto.Password, user.Password);
            if (!ok) return Unauthorized("Invalid credentials");

            var token = _tokenService.GenerateToken(user.UserId, user.Email, user.Role);
            return Ok(new { token });
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> Me()
        {
            var sub = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            if (sub == null) return Unauthorized();

            var userId = Guid.Parse(sub);
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return NotFound();

            return Ok(new { user.UserId, user.Email, user.Name, user.Role });
        }
    }
}
