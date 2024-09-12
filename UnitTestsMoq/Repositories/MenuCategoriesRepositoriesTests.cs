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
    public class MenuCategoryRepositoryTests
    {
        private ApplicationDbContext _context;
        private MenuCategoryRepository _repository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "FoodCartTestDb")
                .Options;

            _context = new ApplicationDbContext(options);

            // Seed the in-memory database with test data
            SeedDatabase();

            _repository = new MenuCategoryRepository(_context);
        }

        private void SeedDatabase()
        {
            var categories = new List<MenuCategory>
            {
                new MenuCategory { CategoryID = 1, CategoryName = "Starters", CategoryDescription = "Appetizers and starters" },
                new MenuCategory { CategoryID = 2, CategoryName = "Main Course", CategoryDescription = "Main dishes" }
            };

            _context.MenuCategories.AddRange(categories);
            _context.SaveChanges();
        }

        [Test]
        public async Task GetAllCategoriesAsync_ShouldReturnAllCategories()
        {
            // Act
            var result = await _repository.GetAllCategoriesAsync();

            // Assert
            Assert.AreEqual(2, result.Count()); // Expecting 2 categories
        }

        [Test]
        public async Task GetCategoryByIdAsync_ShouldReturnCategory()
        {
            // Act
            var result = await _repository.GetCategoryByIdAsync(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.CategoryID);
            Assert.AreEqual("Starters", result.CategoryName);
        }

        [Test]
        public async Task AddCategoryAsync_ShouldAddCategory()
        {
            // Arrange
            var newCategory = new MenuCategory
            {
                CategoryID = 3,
                CategoryName = "Desserts",
                CategoryDescription = "Sweet treats and desserts"
            };

            // Act
            await _repository.AddCategoryAsync(newCategory);
            var result = await _repository.GetCategoryByIdAsync(3);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.CategoryID);
            Assert.AreEqual("Desserts", result.CategoryName);
        }

        [Test]
        public async Task UpdateCategoryAsync_ShouldUpdateCategory()
        {
            // Arrange
            var category = await _repository.GetCategoryByIdAsync(1);
            category.CategoryName = "Appetizers";

            // Act
            await _repository.UpdateCategoryAsync(category);
            var updatedCategory = await _repository.GetCategoryByIdAsync(1);

            // Assert
            Assert.AreEqual("Appetizers", updatedCategory.CategoryName);
        }

        [Test]
        public async Task DeleteCategoryAsync_ShouldRemoveCategory()
        {
            // Act
            await _repository.DeleteCategoryAsync(1);
            var result = await _repository.GetCategoryByIdAsync(1);

            // Assert
            Assert.IsNull(result);
        }
        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }
    }
}
