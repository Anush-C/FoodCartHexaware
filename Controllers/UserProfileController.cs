using FoodCart_Hexaware.Data;
using FoodCart_Hexaware.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging; // Add this using statement
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace FoodCart_Hexaware.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserProfileController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UserProfileController> _logger; // Add logger field

        public UserProfileController(ApplicationDbContext context, ILogger<UserProfileController> logger)
        {
            _context = context;
            _logger = logger; // Initialize logger
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserProfileDTO>> GetUserProfile(int id)
        {
            _logger.LogInformation("Getting user profile for user ID: {UserId}", id);
            var user = await _context.Users
                                     .Where(u => u.UserID == id)
                                     .Select(u => new UserProfileDTO
                                     {
                                         UserID = u.UserID,
                                         UserName = u.UserName,
                                         Email = u.Email,
                                         PhoneNumber = u.PhoneNumber,
                                         Role = u.Role
                                     })
                                     .FirstOrDefaultAsync();

            if (user == null)
            {
                _logger.LogWarning("User not found for ID: {UserId}", id);
                return NotFound(new { message = "User not found" });
            }

            return Ok(user);
        }

        [HttpPut("ChangeEmail/{id}")]
        public async Task<IActionResult> ChangeEmail(int id, [FromBody] string newEmail)
        {
            _logger.LogInformation("Changing email for user ID: {UserId}", id);

            if (!new EmailAddressAttribute().IsValid(newEmail))
            {
                _logger.LogWarning("Invalid email format for user ID: {UserId}", id);
                return BadRequest(new { message = "Invalid email format" });
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                _logger.LogWarning("User not found for ID: {UserId}", id);
                return NotFound(new { message = "User not found" });
            }

            user.Email = newEmail;
            await _context.SaveChangesAsync();
            _logger.LogInformation("Email updated successfully for user ID: {UserId}", id);
            return Ok(new { message = "Email updated successfully" });
        }

        [HttpPut("UpdateAlternativePhone/{id}")]
        public async Task<IActionResult> UpdateAlternativePhone(int id, [FromBody] string newPhoneNumber)
        {
            _logger.LogInformation("Updating alternative phone number for user ID: {UserId}", id);
            var phoneNumberPattern = @"^\d{10}$";
            if (!Regex.IsMatch(newPhoneNumber, phoneNumberPattern))
            {
                _logger.LogWarning("Invalid phone number format for user ID: {UserId}", id);
                return BadRequest(new { message = "Phone number must be 10 digits." });
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                _logger.LogWarning("User not found for ID: {UserId}", id);
                return NotFound(new { message = "User not found" });
            }

            user.AlternativePhoneNumber = newPhoneNumber;
            await _context.SaveChangesAsync();
            _logger.LogInformation("Alternative phone number updated successfully for user ID: {UserId}", id);
            return Ok(new { message = "Alternative phone number updated successfully" });
        }

        [HttpPut("ChangePassword/{id}")]
        public async Task<IActionResult> ChangePassword(int id, [FromBody] ChangePasswordDTO passwordData)
        {
            _logger.LogInformation("Changing password for user ID: {UserId}", id);
            if (passwordData.NewPassword.Length < 8)
            {
                _logger.LogWarning("Password too short for user ID: {UserId}", id);
                return BadRequest(new { message = "Password must be at least 8 characters long" });
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                _logger.LogWarning("User not found for ID: {UserId}", id);
                return NotFound(new { message = "User not found" });
            }

            if (!BCrypt.Net.BCrypt.Verify(passwordData.CurrentPassword, user.Password))
            {
                _logger.LogWarning("Current password incorrect for user ID: {UserId}", id);
                return BadRequest(new { message = "Current password is incorrect" });
            }

            user.Password = BCrypt.Net.BCrypt.HashPassword(passwordData.NewPassword);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Password changed successfully for user ID: {UserId}", id);
            return Ok(new { message = "Password changed successfully" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserProfile(int id, [FromBody] UserProfileDTO updatedUser)
        {
            _logger.LogInformation("Updating user profile for user ID: {UserId}", id);
            if (id != updatedUser.UserID)
            {
                _logger.LogWarning("User ID mismatch for user ID: {UserId}", id);
                return BadRequest(new { message = "User ID mismatch" });
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                _logger.LogWarning("User not found for ID: {UserId}", id);
                return NotFound(new { message = "User not found" });
            }

            user.UserName = updatedUser.UserName;
            user.Email = updatedUser.Email;
            user.PhoneNumber = updatedUser.PhoneNumber;
            user.Role = updatedUser.Role;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    _logger.LogWarning("User not found during update for ID: {UserId}", id);
                    return NotFound(new { message = "User not found" });
                }
                else
                {
                    _logger.LogError("Concurrency error occurred while updating user ID: {UserId}", id);
                    throw;
                }
            }

            _logger.LogInformation("User profile updated successfully for user ID: {UserId}", id);
            return NoContent();
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteUserProfile(int id)
        {
            _logger.LogInformation("Deleting user profile for user ID: {UserId}", id);
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                _logger.LogWarning("User not found for ID: {UserId}", id);
                return NotFound(new { message = "User not found" });
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            _logger.LogInformation("User profile deleted successfully for user ID: {UserId}", id);
            return NoContent();
        }

        [HttpGet("orders/{userId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetUserOrders(int userId)
        {
            _logger.LogInformation("Fetching orders for user ID: {UserId}", userId);
            var ordersWithItems = await _context.Orders
                .Where(o => o.UserID == userId)
                .Select(o => new
                {
                    o.OrderID,
                    o.TotalPrice,
                    o.ShippingAddress,
                    o.OrderStatus,
                    o.OrderTime,
                    Items = _context.OrderItems
                        .Where(oi => oi.OrderID == o.OrderID)
                        .Select(oi => new
                        {
                            oi.Quantity,
                            ItemName = _context.Menus
                                .Where(mi => mi.ItemID == oi.ItemID)
                                .Select(mi => mi.ItemName)
                                .FirstOrDefault()
                        })
                        .ToList()
                })
                .ToListAsync();

            if (ordersWithItems == null || !ordersWithItems.Any())
            {
                _logger.LogWarning("No orders found for user ID: {UserId}", userId);
                return NotFound("No orders found for this user.");
            }

            _logger.LogInformation("Fetched orders successfully for user ID: {UserId}", userId);
            return Ok(ordersWithItems);
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(u => u.UserID == id);
        }
    }
}
