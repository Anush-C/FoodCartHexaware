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
        private readonly ILogger<AuthController> _logger; // Add logger

        public AuthController(IConfiguration configuration, ApplicationDbContext applicationDbContext, ILogger<AuthController> logger)
        {
            _configuration = configuration;
            _context = applicationDbContext;
            _logger = logger; // Initialize logger
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            try
            {
                var errorMessages = new List<string>();

                // Check if the username already exists
                if (await _context.Users.AnyAsync(u => u.UserName == registerDTO.UserName))
                {
                    errorMessages.Add("Username already exists.");
                }

                // Check if the email already exists
                if (await _context.Users.AnyAsync(u => u.Email == registerDTO.Email))
                {
                    errorMessages.Add("Email already exists.");
                }

                // Validate password length
                if (string.IsNullOrEmpty(registerDTO.Password) || registerDTO.Password.Length < 8)
                {
                    errorMessages.Add("Password must contain at least 8 characters.");
                }

                // Handle role-specific validation
                switch (registerDTO.Role)
                {
                    case "Hotel Owner":
                        if (!registerDTO.RestaurantID.HasValue)
                        {
                            errorMessages.Add("Restaurant ID is required for Hotel Owner.");
                        }
                        else if (!await _context.Restaurants.AnyAsync(r => r.RestaurantID == registerDTO.RestaurantID))
                        {
                            errorMessages.Add("Invalid Restaurant ID.");
                        }
                        break;

                    case "Customer":
                    case "Admin":
                        if (registerDTO.RestaurantID.HasValue)
                        {
                            errorMessages.Add("Restaurant ID should not be provided for Customer/Admin.");
                        }
                        break;

                    default:
                        errorMessages.Add("Invalid role specified.");
                        break;
                }

                // If there are any validation errors, return them
                if (errorMessages.Any())
                {
                    return BadRequest(string.Join(" ", errorMessages)); // Simplified error message response
                }

                // Create the user entity
                var user = new Users
                {
                    UserName = registerDTO.UserName,
                    Email = registerDTO.Email,
                    Password = BCrypt.Net.BCrypt.HashPassword(registerDTO.Password),
                    PhoneNumber = registerDTO.PhoneNumber,
                    Role = registerDTO.Role,
                    RestaurantID = registerDTO.RestaurantID // Optional based on role
                };

                // Add user to the database
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"User registered successfully: {user.UserName}"); // Log registration success
                return Ok("User Registered Successfully !!");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError($"Database error occurred while registering user: {ex.InnerException?.Message ?? ex.Message}");
                return StatusCode(500, $"A database error occurred while registering the user: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred during registration: {ex.Message}");
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
                    _logger.LogWarning($"Login attempt failed: User with email {loginDTO.Email} does not exist."); // Log warning for failed login
                    return BadRequest("User with this email does not exist."); // Clear message for user not found
                }

                // Validate the password
                bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDTO.Password, user.Password);
                if (!isPasswordValid)
                {
                    _logger.LogWarning($"Login attempt failed for user {user.UserName}: Invalid password."); // Log warning for invalid password
                    return BadRequest("Invalid Credentials: Password mismatch."); // Clear message for password mismatch
                }

                // Create claims based on user role and other information
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("UserId", user.UserID.ToString()),
                    new Claim(ClaimTypes.Role, user.Role),
                    new Claim("RestaurantID", user.RestaurantID?.ToString() ?? string.Empty)
                };

                // Define the security key and signing credentials
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                // Create the JWT token
                var token = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    claims,
                    expires: DateTime.UtcNow.AddHours(2),
                    signingCredentials: signIn
                );

                // Log successful login
                _logger.LogInformation($"User {user.UserName} logged in successfully.");
                // Return the token and additional user details
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                    role = user.Role,
                    restaurantId = user.RestaurantID
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred during login: {ex.Message}");
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
