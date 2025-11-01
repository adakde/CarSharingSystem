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
        // === ADMIN: POBIERZ WSZYSTKICH UŻYTKOWNIKÓW ===
        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _context.Users
                .Select(u => new
                {
                    u.UserId,
                    u.Name,
                    u.Email,
                    u.Role
                })
                .ToListAsync();

            return Ok(users);
        }

        // === ADMIN: ZMIEŃ ROLĘ UŻYTKOWNIKA ===
        [HttpPut("{id:guid}/role")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChangeRole(Guid id, [FromBody] string newRole)
        {
            if (!Enum.TryParse<UserRole>(newRole, out var role))
                return BadRequest("Invalid role");

            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound("User not found");

            user.Role = role;
            await _context.SaveChangesAsync();

            return Ok(new { message = $"Role changed to {newRole}" });
        }

        // === ADMIN: USUŃ UŻYTKOWNIKA ===
        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound("User not found");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "User deleted" });
        }

    }
}
