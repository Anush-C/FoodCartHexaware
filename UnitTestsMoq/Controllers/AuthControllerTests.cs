using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using FoodCart_Hexaware.Controllers;
using FoodCart_Hexaware.Data;
using FoodCart_Hexaware.DTO;
using FoodCart_Hexaware.Models;
using BCrypt.Net;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using FluentAssertions;

namespace UnitTestsMoq.Controllers
{
    [TestFixture]
    public class AuthControllerTests
    {
        private AuthController _authController;
        private ApplicationDbContext _context;
        private IConfiguration _configuration;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "FoodCartTestDb")
                .Options;

            _context = new ApplicationDbContext(options);

            // Set up in-memory configuration
            var configurationBuilder = new ConfigurationBuilder()
                .AddInMemoryCollection(new[]
                {
                    new KeyValuePair<string, string>("Jwt:Key", "ThisIsASecretKeyForTesting"),
                    new KeyValuePair<string, string>("Jwt:Issuer", "YourIssuer"),
                    new KeyValuePair<string, string>("Jwt:Audience", "YourAudience")
                });

            _configuration = configurationBuilder.Build();

            _authController = new AuthController(_configuration, _context);
        }
        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task Register_ShouldReturnBadRequest_WhenEmailAlreadyExists()
        {
            // Arrange
            var registerDTO = new RegisterDTO
            {
                UserName = "testuser",
                Email = "test@example.com",
                Password = "password123",
                PhoneNumber = "1234567890",
                Role = "Customer",
                RestaurantID = null
            };

            _context.Users.Add(new Users
            {
                UserName = "existinguser",
                Email = "test@example.com",
                Password = BCrypt.Net.BCrypt.HashPassword("password123"),
                PhoneNumber = "1234567890",
                Role = "Customer"
            });

            await _context.SaveChangesAsync();

            // Act
            var result = await _authController.Register(registerDTO);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                .Which.StatusCode.Should().Be(400);
        }

        [Test]
        public async Task Register_ShouldReturnOk_WhenUserIsRegisteredSuccessfully()
        {
            // Arrange
            var registerDTO = new RegisterDTO
            {
                UserName = "testuser",
                Email = "newuser@example.com",
                Password = "password123",
                PhoneNumber = "1234567890",
                Role = "Customer",
                RestaurantID = null
            };

            // Act
            var result = await _authController.Register(registerDTO);

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                .Which.StatusCode.Should().Be(200);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == registerDTO.Email);
            user.Should().NotBeNull();
            BCrypt.Net.BCrypt.Verify(registerDTO.Password, user.Password).Should().BeTrue();
        }

        [Test]
        public async Task Login_ShouldReturnUnauthorized_WhenUserNotFound()
        {
            // Arrange
            var loginDTO = new LoginDTO
            {
                Email = "nonexistentuser@example.com",
                Password = "password123"
            };

            // Act
            var result = await _authController.Login(loginDTO);

            // Assert
            result.Should().BeOfType<UnauthorizedObjectResult>()
                .Which.StatusCode.Should().Be(401);
        }

        


    }
}
