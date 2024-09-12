using FoodCart_Hexaware.Data;
using FoodCart_Hexaware.DTO;
using FoodCart_Hexaware.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FoodCart_Hexaware.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public AuthController(IConfiguration configuration, ApplicationDbContext applicationDbContext)
        {
            _configuration = configuration;
            _context = applicationDbContext;

        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            try
            {
                if (await _context.Users.AnyAsync(u => u.Email == registerDTO.Email))
                {
                    return BadRequest("Email already exists.");
                }

                if (registerDTO.Role == "HotelOwner" && !registerDTO.RestaurantID.HasValue)
                {
                    return BadRequest("Restaurant ID is required for Hotel Owner.");
                }

                var user = new Users
                {
                    UserName = registerDTO.UserName,
                    Email = registerDTO.Email,
                    Password = BCrypt.Net.BCrypt.HashPassword(registerDTO.Password),
                    PhoneNumber = registerDTO.PhoneNumber,
                    Role = registerDTO.Role,
                    RestaurantID = registerDTO.RestaurantID // Save RestaurantID if HotelOwner
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return Ok("User Registered Successfully !!");
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, "A database error occurred while registering the user. Please try again.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDTO.Email);
                if (user == null)
                {
                    return Unauthorized("Invalid Credentials: User not found.");
                }

                bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDTO.Password, user.Password);
                if (!isPasswordValid)
                {
                    return Unauthorized("Invalid Credentials: Password mismatch.");
                }

                var claims = new[]
                {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("UserId", user.UserID.ToString()),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim("RestaurantID", user.RestaurantID?.ToString() ?? "") // Add RestaurantID claim
        };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    claims,
                    expires: DateTime.UtcNow.AddHours(2),
                    signingCredentials: signIn
                );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                    role = user.Role,
                    restaurantId = user.RestaurantID // Include RestaurantID in response
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

    }
}
