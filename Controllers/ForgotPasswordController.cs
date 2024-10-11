using FoodCart_Hexaware.Data;
using FoodCart_Hexaware.DTO;
using FoodCart_Hexaware.Models;
using FoodCart_Hexaware.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging; // Add this using directive
using System;
using System.Threading.Tasks;

namespace FoodCart_Hexaware.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ForgotPasswordController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;
        private readonly ILogger<ForgotPasswordController> _logger; // Declare the logger

        public ForgotPasswordController(ApplicationDbContext context, IEmailService emailService, ILogger<ForgotPasswordController> logger) // Inject the logger
        {
            _context = context;
            _emailService = emailService;
            _logger = logger; // Initialize the logger
        }

        [HttpPost("request-reset")]
        public async Task<IActionResult> RequestPasswordReset(ForgotPasswordDTO forgotPasswordDTO)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == forgotPasswordDTO.Email);
            if (user == null)
            {
                _logger.LogWarning("Password reset requested for non-existent email: {Email}", forgotPasswordDTO.Email);
                return BadRequest(new { error = "No user found with this email." });
            }

            // Generate reset token
            var resetToken = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

            // Create a PasswordResetToken entity
            var passwordResetToken = new PasswordResetToken
            {
                Token = resetToken,
                UserID = user.UserID,
                ExpiryDate = DateTime.UtcNow.AddHours(1)
            };

            _context.PasswordResetToken.Add(passwordResetToken);
            await _context.SaveChangesAsync();

            // Generate reset link (to be included in the email)
            var resetLink = $"http://localhost:3001/reset-password?token={resetToken}";

            // Use the email service to send the reset link
            var subject = "Password Reset Request";
            var message = $@"
            <!DOCTYPE html>
            <html lang='en'>
            <head>
                <meta charset='UTF-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <title>Password Reset</title>
                <style>
                    body {{
                        font-family: Arial, sans-serif;
                        background-color: #f4f4f4;
                        margin: 0;
                        padding: 20px;
                    }}
                    .container {{
                        background-color: #ffffff;
                        padding: 20px;
                        border-radius: 5px;
                        box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                    }}
                    h2 {{
                        color: #333;
                    }}
                    a {{
                        color: #007BFF;
                        text-decoration: none;
                    }}
                    .footer {{
                        margin-top: 20px;
                        font-size: 12px;
                        color: #777;
                    }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <h2>Password Reset Request</h2>
                    <p>Hi {user.UserName},</p>
                    <p>We received a request to reset your password. You can reset your password by clicking the link below:</p>
                    <p><a href='{resetLink}'>Reset Your Password</a></p>
                    <p>This link will expire in 1 hour.</p>
                    <p>If you did not request a password reset, please ignore this email.</p>
                    <p>Thank you!</p>
                    <div class='footer'>
                        <p>&copy; {DateTime.UtcNow.Year} Food Cart. All rights reserved.</p>
                    </div>
                </div>
            </body>
            </html>";

            // Send the email
            await _emailService.SendEmailAsync(user.Email, subject, message, true); // Pass 'true' for HTML content

            _logger.LogInformation("Password reset link sent to {Email}", user.Email);
            return Ok(new { message = "Password reset link has been sent to the provided email address." });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO resetPasswordDTO)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(resetPasswordDTO.NewPassword) ||
                string.IsNullOrWhiteSpace(resetPasswordDTO.ConfirmPassword))
            {
                _logger.LogWarning("Reset password request with empty new password or confirmation.");
                return BadRequest(new { error = "New password and confirmation cannot be empty." });
            }

            // Ensure passwords match
            if (resetPasswordDTO.NewPassword != resetPasswordDTO.ConfirmPassword)
            {
                _logger.LogWarning("Password reset request for user with token {Token} - passwords do not match.", resetPasswordDTO.Token);
                return BadRequest(new { error = "New password and confirm password do not match." });
            }

            // Validate the token
            var resetToken = await _context.PasswordResetToken.FirstOrDefaultAsync(t => t.Token == resetPasswordDTO.Token);
            if (resetToken == null || resetToken.ExpiryDate < DateTime.UtcNow)
            {
                _logger.LogWarning("Invalid or expired token used for password reset: {Token}", resetPasswordDTO.Token);
                return BadRequest(new { error = "Invalid or expired token." });
            }

            // Find the user
            var user = await _context.Users.FindAsync(resetToken.UserID);
            if (user == null)
            {
                _logger.LogWarning("No user found for password reset token: {Token}", resetPasswordDTO.Token);
                return BadRequest(new { error = "User not found." });
            }

            // Update the password
            user.Password = BCrypt.Net.BCrypt.HashPassword(resetPasswordDTO.NewPassword);
            _context.PasswordResetToken.Remove(resetToken); // Remove the used token
            await _context.SaveChangesAsync();

            _logger.LogInformation("Password reset successful for user {UserId}", user.UserID);
            return Ok(new { message = "Password has been reset successfully." });
        }
    }
}
