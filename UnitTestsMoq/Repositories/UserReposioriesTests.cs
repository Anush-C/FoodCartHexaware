using FoodCart_Hexaware.Data;
using FoodCart_Hexaware.Models;
using FoodCart_Hexaware.Repositories;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UnitTestsMoq.Repositories
{
    [TestFixture]
    public class UserRepositoryTests
    {
        private ApplicationDbContext _context;
        private UserRepository _userRepository;
        private List<Users> _userList;

        [SetUp]
        public void Setup()
        {
            // Create a new in-memory database and configure the context
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ApplicationDbContext(options);

            // Seed initial data
            _userList = new List<Users>
            {
                new Users { UserID = 1, UserName = "User1", Email = "user1@example.com", Password = "Password123", PhoneNumber = "123-456-7890", Role = "Customer" },
                new Users { UserID = 2, UserName = "User2", Email = "user2@example.com", Password = "Password123", PhoneNumber = "987-654-3210", Role = "Admin" },
                new Users { UserID = 3, UserName = "User3", Email = "user3@example.com", Password = "Password123", PhoneNumber = "111-222-3333", Role = "HotelOwner" },
                new Users { UserID = 4, UserName = "User4", Email = "user4@example.com", Password = "Password123", PhoneNumber = "444-555-6666", Role = "Customer" },
                new Users { UserID = 5, UserName = "User5", Email = "user5@example.com", Password = "Password123", PhoneNumber = "777-888-9999", Role = "Admin" }
            };

            _context.Users.AddRange(_userList);
            _context.SaveChanges();

            _userRepository = new UserRepository(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task GetAllUsersAsync_ShouldReturnAllUsers()
        {
            // Act
            var users = await _userRepository.GetAllUsersAsync();

            // Assert
            Assert.AreEqual(5, users.Count(), "The number of users returned should be 5.");
        }

        [Test]
        public async Task GetUserByIdAsync_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var userId = 1;

            // Act
            var user = await _userRepository.GetUserByIdAsync(userId);

            // Assert
            Assert.IsNotNull(user, "User should not be null.");
            Assert.AreEqual("User1", user.UserName, "UserName should match.");
        }

        [Test]
        public async Task AddUserAsync_ShouldAddUser()
        {
            // Arrange
            var newUser = new Users
            {
                UserID = 6,
                UserName = "User6",
                Email = "user6@example.com",
                Password = "Password123",
                PhoneNumber = "000-111-2222",
                Role = "Customer"
            };

            // Act
            await _userRepository.AddUserAsync(newUser);
            var user = await _userRepository.GetUserByIdAsync(6);

            // Assert
            Assert.IsNotNull(user, "Newly added user should not be null.");
            Assert.AreEqual("User6", user.UserName, "UserName should match.");
        }

        [Test]
        public async Task UpdateUserAsync_ShouldUpdateUser()
        {
            // Arrange
            var user = await _userRepository.GetUserByIdAsync(1);
            user.UserName = "UpdatedUser1";

            // Act
            await _userRepository.UpdateUserAsync(user);
            var updatedUser = await _userRepository.GetUserByIdAsync(1);

            // Assert
            Assert.AreEqual("UpdatedUser1", updatedUser.UserName, "UserName should be updated.");
        }

        [Test]
        public async Task DeleteUserAsync_ShouldRemoveUser_WhenUserExists()
        {
            // Arrange
            var userId = 1;

            // Act
            await _userRepository.DeleteUserAsync(userId);
            var user = await _userRepository.GetUserByIdAsync(userId);

            // Assert
            Assert.IsNull(user, "User should be removed from the database.");
        }
    }
}
