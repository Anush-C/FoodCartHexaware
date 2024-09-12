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
    public class MenuItemRepositoryTests
    {
        private ApplicationDbContext _context;
        private MenuItemRepository _repository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "FoodCartTestDb")
                .Options;

            _context = new ApplicationDbContext(options);

            // Seed the in-memory database with test data
            SeedDatabase();

            _repository = new MenuItemRepository(_context);
        }

        private void SeedDatabase()
        {
            var menuCategory = new MenuCategory
            {
                CategoryID = 1,
                CategoryName = "Main Course",
                CategoryDescription = "Main course dishes"
            };

            var menuItem1 = new MenuItems
            {
                ItemID = 1,
                ItemName = "Pizza",
                ItemDescription = "Delicious cheese pizza",
                ItemPrice = 12.99m,
                Ingredients = "Cheese, Tomato Sauce, Dough",
                CuisineType = "Italian",
                TasteInfo = "Savory",
                AvailabilityStatus = "Available",
                DietaryInfo = "Vegetarian",
                CategoryID = 1,
                ImageURL = "https://example.com/pizza.jpg"
            };

            var menuItem2 = new MenuItems
            {
                ItemID = 2,
                ItemName = "Pasta",
                ItemDescription = "Creamy Alfredo pasta",
                ItemPrice = 14.99m,
                Ingredients = "Cream, Cheese, Pasta",
                CuisineType = "Italian",
                TasteInfo = "Savory",
                AvailabilityStatus = "Available",
                DietaryInfo = "Vegetarian",
                CategoryID = 1,
                ImageURL = "https://example.com/pasta.jpg"
            };

            _context.MenuCategories.Add(menuCategory);
            _context.Menus.AddRange(menuItem1, menuItem2);
            _context.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task GetAllMenuItemsAsync_ShouldReturnAllMenuItems()
        {
            // Act
            var result = await _repository.GetAllMenuItemsAsync();

            // Assert
            Assert.AreEqual(2, result.Count()); // Expecting 2 items
        }

        [Test]
        public async Task GetMenuItemByIdAsync_ShouldReturnMenuItem()
        {
            // Act
            var result = await _repository.GetMenuItemByIdAsync(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.ItemID);
            Assert.AreEqual("Pizza", result.ItemName);
        }

        [Test]
        public async Task AddMenuItemAsync_ShouldAddMenuItem()
        {
            // Arrange
            var newMenuItem = new MenuItems
            {
                ItemID = 3,
                ItemName = "Burger",
                ItemDescription = "Juicy beef burger",
                ItemPrice = 10.99m,
                Ingredients = "Beef, Lettuce, Tomato, Bun",
                CuisineType = "American",
                TasteInfo = "Savory",
                AvailabilityStatus = "Available",
                DietaryInfo = "Non-Vegetarian",
                CategoryID = 1,
                ImageURL = "https://example.com/burger.jpg"
            };

            // Act
            await _repository.AddMenuItemAsync(newMenuItem);
            var result = await _repository.GetMenuItemByIdAsync(3);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.ItemID);
            Assert.AreEqual("Burger", result.ItemName);
        }

        [Test]
        public async Task UpdateMenuItemAsync_ShouldUpdateMenuItem()
        {
            // Arrange
            var menuItem = await _repository.GetMenuItemByIdAsync(1);
            menuItem.ItemPrice = 15.99m;

            // Act
            await _repository.UpdateMenuItemAsync(menuItem);
            var updatedMenuItem = await _repository.GetMenuItemByIdAsync(1);

            // Assert
            Assert.AreEqual(15.99m, updatedMenuItem.ItemPrice);
        }

        [Test]
        public async Task DeleteMenuItemAsync_ShouldRemoveMenuItem()
        {
            // Act
            await _repository.DeleteMenuItemAsync(1);
            var result = await _repository.GetMenuItemByIdAsync(1);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task UpdateAvailabilityStatusAsync_ShouldUpdateStatus()
        {
            // Act
            await _repository.UpdateAvailabilityStatusAsync(1, "Unavailable");
            var updatedMenuItem = await _repository.GetMenuItemByIdAsync(1);

            // Assert
            Assert.AreEqual("Unavailable", updatedMenuItem.AvailabilityStatus);
        }
    }
}
