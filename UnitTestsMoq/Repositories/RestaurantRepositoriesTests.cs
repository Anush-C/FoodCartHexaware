using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using FoodCart_Hexaware.Data;
using FoodCart_Hexaware.Models;
using FoodCart_Hexaware.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UnitTestsMoq.Repositories
{
    [TestFixture]
    public class RestaurantRepositoryTests
    {
        private ApplicationDbContext _context;
        private RestaurantRepository _repository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "FoodCartDatabase")
                .Options;

            _context = new ApplicationDbContext(options);

            // Seed the database with some test data
            _context.Restaurants.AddRange(new List<Restaurant>
            {
                new Restaurant
                {
                    RestaurantID = 1,
                    RestaurantName = "Restaurant1",
                    RestaurantDescription = "Description1",
                    RestaurantPhone = "123-456-7890",
                    RestaurantEmail = "restaurant1@example.com",
                    RestaurantAddress = "Address1",
                    OpeningHours = new TimeSpan(9, 0, 0),
                    ClosingHours = new TimeSpan(22, 0, 0)
                },
                new Restaurant
                {
                    RestaurantID = 2,
                    RestaurantName = "Restaurant2",
                    RestaurantDescription = "Description2",
                    RestaurantPhone = "098-765-4321",
                    RestaurantEmail = "restaurant2@example.com",
                    RestaurantAddress = "Address2",
                    OpeningHours = new TimeSpan(10, 0, 0),
                    ClosingHours = new TimeSpan(23, 0, 0)
                },
                new Restaurant
                {
                    RestaurantID = 3,
                    RestaurantName = "Restaurant3",
                    RestaurantDescription = "Description3",
                    RestaurantPhone = "111-222-3333",
                    RestaurantEmail = "restaurant3@example.com",
                    RestaurantAddress = "Address3",
                    OpeningHours = new TimeSpan(8, 0, 0),
                    ClosingHours = new TimeSpan(20, 0, 0)
                },
                new Restaurant
                {
                    RestaurantID = 4,
                    RestaurantName = "Restaurant4",
                    RestaurantDescription = "Description4",
                    RestaurantPhone = "444-555-6666",
                    RestaurantEmail = "restaurant4@example.com",
                    RestaurantAddress = "Address4",
                    OpeningHours = new TimeSpan(11, 0, 0),
                    ClosingHours = new TimeSpan(22, 0, 0)
                }
            });

            _context.SaveChanges();

            _repository = new RestaurantRepository(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task GetAllRestaurantsAsync_ShouldReturnAllRestaurants()
        {
            // Act
            var result = await _repository.GetAllRestaurantsAsync();

            // Assert
            Assert.AreEqual(4, result.Count(), "The number of restaurants returned should be 4.");
            Assert.AreEqual("Restaurant1", result.First().RestaurantName, "The first restaurant should be Restaurant1.");
        }

        [Test]
        public async Task GetRestaurantByIdAsync_ShouldReturnCorrectRestaurant()
        {
            // Act
            var result = await _repository.GetRestaurantByIdAsync(1);

            // Assert
            Assert.IsNotNull(result, "Restaurant should not be null.");
            Assert.AreEqual("Restaurant1", result.RestaurantName, "RestaurantName should match.");
        }

        [Test]
        public async Task AddRestaurantAsync_ShouldAddRestaurant()
        {
            // Arrange
            var newRestaurant = new Restaurant
            {
                RestaurantID = 5,
                RestaurantName = "Restaurant5",
                RestaurantDescription = "Description5",
                RestaurantPhone = "777-888-9999",
                RestaurantEmail = "restaurant5@example.com",
                RestaurantAddress = "Address5",
                OpeningHours = new TimeSpan(7, 0, 0),
                ClosingHours = new TimeSpan(21, 0, 0)
            };

            // Act
            await _repository.AddRestaurantAsync(newRestaurant);

            // Assert
            var addedRestaurant = await _repository.GetRestaurantByIdAsync(5);
            Assert.IsNotNull(addedRestaurant, "Newly added restaurant should not be null.");
            Assert.AreEqual("Restaurant5", addedRestaurant.RestaurantName, "RestaurantName should match.");
        }

        [Test]
        public async Task UpdateRestaurantAsync_ShouldUpdateRestaurant()
        {
            // Arrange
            var existingRestaurant = await _repository.GetRestaurantByIdAsync(1);
            existingRestaurant.RestaurantName = "UpdatedRestaurant1";
            existingRestaurant.RestaurantDescription = "UpdatedDescription1";
            existingRestaurant.RestaurantPhone = "321-654-9870";
            existingRestaurant.RestaurantEmail = "updated@example.com";
            existingRestaurant.RestaurantAddress = "UpdatedAddress1";
            existingRestaurant.OpeningHours = new TimeSpan(10, 0, 0);
            existingRestaurant.ClosingHours = new TimeSpan(23, 0, 0);

            // Act
            await _repository.UpdateRestaurantAsync(existingRestaurant);

            // Assert
            var updatedRestaurant = await _repository.GetRestaurantByIdAsync(1);
            Assert.IsNotNull(updatedRestaurant, "Updated restaurant should not be null.");
            Assert.AreEqual("UpdatedRestaurant1", updatedRestaurant.RestaurantName, "RestaurantName should be updated.");
        }

        [Test]
        public async Task DeleteRestaurantAsync_ShouldRemoveRestaurant()
        {
            // Act
            await _repository.DeleteRestaurantAsync(1);

            // Assert
            var deletedRestaurant = await _repository.GetRestaurantByIdAsync(1);
            Assert.IsNull(deletedRestaurant, "Restaurant should be removed from the database.");
        }
    }
}
