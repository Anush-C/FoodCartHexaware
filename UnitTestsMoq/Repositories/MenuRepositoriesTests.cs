using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodCart_Hexaware.Data;
using FoodCart_Hexaware.Models;
using FoodCart_Hexaware.Repositories;
using Microsoft.EntityFrameworkCore;

namespace UnitTestsMoq.Repositories
{
    [TestFixture]
    public class MenuRepositoryTests
    {
        private ApplicationDbContext _context;
        private MenuRepository _repository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "FoodCartTestDb")
                .Options;

            _context = new ApplicationDbContext(options);

            // Setup test data for MenuCategory
            var menuCategory = new MenuCategory
            {
                CategoryID = 1,
                CategoryName = "Main Course",
                CategoryDescription = "Main course dishes"
            };

            // Setup test data for Restaurant with all required properties
            var restaurant = new Restaurant
            {
                RestaurantID = 1,
                RestaurantName = "Italian Bistro",
                RestaurantDescription = "A cozy Italian restaurant.",
                RestaurantPhone = "555-1234",
                RestaurantEmail = "contact@italianbistro.com",
                RestaurantAddress = "123 Pasta Lane",
                OpeningHours = new TimeSpan(10, 0, 0), // 10:00 AM
                ClosingHours = new TimeSpan(22, 0, 0)  // 10:00 PM
            };

            // Setup test data for MenuItems
            var menuItems = new List<MenuItems>
            {
                new MenuItems
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
                },
                new MenuItems
                {
                    ItemID = 2,
                    ItemName = "Burger",
                    ItemDescription = "Juicy beef burger",
                    ItemPrice = 8.99m,
                    Ingredients = "Beef, Lettuce, Tomato, Bun",
                    CuisineType = "American",
                    TasteInfo = "Savory",
                    AvailabilityStatus = "Available",
                    DietaryInfo = "Non-Vegetarian",
                    CategoryID = 1,
                    ImageURL = "https://example.com/burger.jpg"
                }
            };

            // Add entities to context
            _context.MenuCategories.Add(menuCategory);
            _context.Restaurants.Add(restaurant);
            _context.Menus.AddRange(menuItems);

            // Save changes to the in-memory database
            _context.SaveChanges();

            // Instantiate repository
            _repository = new MenuRepository(_context);
        }

        [Test]
        public async Task GetMenuItem_ShouldReturnAllMenuItems()
        {
            // Act
            var result = await _repository.GetMenuItem();

            // Assert
            Assert.AreEqual(2, result.Count()); // Ensure that there are 2 items returned
        }

        [Test]
        public async Task GetMenuItembyId_ShouldReturnMenuItem()
        {
            var result = await _repository.GetMenuItembyId(1);
            Assert.IsNotNull(result);
            Assert.AreEqual("Pizza", result.ItemName);
        }

        [Test]
        public async Task GetMenuItembyName_ShouldReturnMenuItem()
        {
            var result = await _repository.GetMenuItembyName("Pizza");
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("Pizza", result.First().ItemName);
        }

        [Test]
        public async Task GetMenuItembyCategory_ShouldReturnMenuItems()
        {
            var result = await _repository.GetMenuItembyCategory("Main Course");
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public async Task GetMenuItemsbyCuisine_ShouldReturnMenuItems()
        {
            var result = await _repository.GetMenuItemsbyCuisine("American");
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("Burger", result.First().ItemName);
        }

        [Test]
        public async Task GetMenuItemsbyAvailability_ShouldReturnAvailableItems()
        {
            var result = await _repository.GetMenuItemsbyAvailability();
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public async Task GetMenuItemsbyPrice_ShouldReturnMenuItemsWithinPriceRange()
        {
            var result = await _repository.GetMenuItemsbyPrice(15, 5);
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public async Task GetMenuItemsbySearch_ShouldReturnMenuItemsBySearchQuery()
        {
            var result = await _repository.GetMenuItemsbySearch("Pizza");
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("Pizza", result.First().ItemName);
        }

        [Test]
        public async Task GetMenuItemsByFilters_ShouldReturnFilteredMenuItems()
        {
            var result = await _repository.GetMenuItemsByFilters("Vegetarian", "Main Course", 5, 15, "Italian");
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("Pizza", result.First().ItemName);
        }

        [Test]
        public async Task LinkMenuItemToRestaurant_ShouldAddMenuItemToRestaurant()
        {
            var result = await _repository.LinkMenuItemToRestaurant(1, 1);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Restaurants.Count);
            Assert.AreEqual(1, result.Restaurants.First().RestaurantID);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }
    }
}
