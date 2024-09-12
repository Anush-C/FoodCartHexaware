using FoodCart_Hexaware.Controllers;
using FoodCart_Hexaware.Data;
using FoodCart_Hexaware.DTO;
using FoodCart_Hexaware.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;
using System.Linq;

namespace UnitTestsMoq.Controllers
{
    [TestFixture]
    public class UserProfileControllerTests
    {
        private ApplicationDbContext _context;
        private UserProfileController _controller;

        [SetUp]
        public void Setup()
        {
            // Create an in-memory database
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "FoodCartTestDb")
                .Options;

            // Initialize the context
            _context = new ApplicationDbContext(options);

            // Seed initial data
            _context.Users.Add(new Users
            {
                UserID = 1,
                UserName = "JohnDoe",
                Email = "john.doe@example.com",
                PhoneNumber = "1234567890",
                Password = BCrypt.Net.BCrypt.HashPassword("Password123"),
                Role = "Customer"
            });
            _context.SaveChanges();

            // Initialize the controller
            _controller = new UserProfileController(_context);
        }
        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task GetUserProfile_ShouldReturnUserProfile_WhenUserExists()
        {
            // Act
            var result = await _controller.GetUserProfile(1);

            // Assert
            var actionResult = result as ActionResult<UserProfileDTO>;
            Assert.IsNotNull(actionResult);
            var okResult = actionResult.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnedUser = okResult.Value as UserProfileDTO;
            Assert.IsNotNull(returnedUser);
            Assert.AreEqual(1, returnedUser.UserID);
            Assert.AreEqual("JohnDoe", returnedUser.UserName);
        }

        [Test]
        public async Task ChangeEmail_ShouldUpdateEmail_WhenEmailIsValid()
        {
            // Act
            var result = await _controller.ChangeEmail(1, "new.email@example.com");

            // Assert
            var actionResult = result as IActionResult;
            Assert.IsInstanceOf<OkObjectResult>(actionResult);

            // Verify the email was updated
            var user = await _context.Users.FindAsync(1);
            Assert.AreEqual("new.email@example.com", user.Email);
        }

        [Test]
        public async Task UpdateAlternativePhone_ShouldUpdatePhoneNumber_WhenPhoneNumberIsValid()
        {
            // Act
            var result = await _controller.UpdateAlternativePhone(1, "0987654321");

            // Assert
            var actionResult = result as IActionResult;
            Assert.IsInstanceOf<OkObjectResult>(actionResult);

            // Verify the phone number was updated
            var user = await _context.Users.FindAsync(1);
            Assert.AreEqual("0987654321", user.AlternativePhoneNumber);
        }

        [Test]
        public async Task ChangePassword_ShouldUpdatePassword_WhenCurrentPasswordIsCorrect()
        {
            // Arrange
            var passwordData = new ChangePasswordDTO
            {
                CurrentPassword = "Password123",
                NewPassword = "NewPassword123"
            };

            // Act
            var result = await _controller.ChangePassword(1, passwordData);

            // Assert
            var actionResult = result as IActionResult;
            Assert.IsInstanceOf<OkObjectResult>(actionResult);

            // Verify the password was updated
            var user = await _context.Users.FindAsync(1);
            Assert.IsTrue(BCrypt.Net.BCrypt.Verify("NewPassword123", user.Password));
        }

        [Test]
        public async Task UpdateUserProfile_ShouldUpdateProfile_WhenUserExists()
        {
            // Arrange
            var updatedUser = new UserProfileDTO
            {
                UserID = 1,
                UserName = "JohnUpdated",
                Email = "john.updated@example.com",
                PhoneNumber = "1112223333",
                Role = "Admin"
            };

            // Act
            var result = await _controller.UpdateUserProfile(1, updatedUser);

            // Assert
            var actionResult = result as IActionResult;
            Assert.IsInstanceOf<NoContentResult>(actionResult);

            // Verify the user profile was updated
            var user = await _context.Users.FindAsync(1);
            Assert.AreEqual("JohnUpdated", user.UserName);
            Assert.AreEqual("john.updated@example.com", user.Email);
            Assert.AreEqual("1112223333", user.PhoneNumber);
            Assert.AreEqual("Admin", user.Role);
        }

        [Test]
        public async Task DeleteUserProfile_ShouldRemoveUser_WhenUserExists()
        {
            // Act
            var result = await _controller.DeleteUserProfile(1);

            // Assert
            var actionResult = result as IActionResult;
            Assert.IsInstanceOf<NoContentResult>(actionResult);

            // Verify the user was removed
            var user = await _context.Users.FindAsync(1);
            Assert.IsNull(user);
        }
    }
}
