using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using FoodCart_Hexaware.Data;
using FoodCart_Hexaware.Models;
using FoodCart_Hexaware.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UnitTestsMoq.Repositories
{
    [TestFixture]
    public class CartRepositoryTests
    {
        private ApplicationDbContext _context;
        private CartRepository _repository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "FoodCartTestDb")
                .Options;

            _context = new ApplicationDbContext(options);

            // Seed the database with some test data
            var carts = new List<Cart>
            {
                new Cart
                {
                    CartID = 1,
                    Quantity = 2,
                    DeliveryAddress = "Address1",
                    TotalCost = 20.00m,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    ItemID = 1,
                    UserID = 1
                },
                new Cart
                {
                    CartID = 2,
                    Quantity = 1,
                    DeliveryAddress = "Address2",
                    TotalCost = 15.00m,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    ItemID = 2,
                    UserID = 2
                }
            };

            _context.Carts.AddRange(carts);
            _context.SaveChanges();

            _repository = new CartRepository(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task GetAllCartItemsAsync_ShouldReturnAllCartItems()
        {
            // Act
            var result = await _repository.GetAllCartItemsAsync();

            // Assert
            Assert.AreEqual(2, result.Count()); // Ensure there are 2 items
            Assert.AreEqual("Address1", result.First().DeliveryAddress); // Ensure the first item's address is correct
        }

        [Test]
        public async Task GetCartItemByIdAsync_ShouldReturnCorrectCartItem()
        {
            // Act
            var result = await _repository.GetCartItemByIdAsync(1);

            // Debugging: Output the result to ensure it is being retrieved
            TestContext.WriteLine($"Result: {result?.DeliveryAddress}");

            // Assert
            Assert.IsNotNull(result, "Expected cart item to be not null.");
            Assert.AreEqual("Address1", result.DeliveryAddress);
        }

        [Test]
        public async Task AddToCartAsync_ShouldAddCartItem()
        {
            // Arrange
            var newCart = new Cart
            {
                CartID = 3,
                Quantity = 3,
                DeliveryAddress = "Address3",
                TotalCost = 30.00m,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                ItemID = 3,
                UserID = 3
            };

            // Act
            var addedCart = await _repository.AddToCartAsync(newCart);

            // Assert
            Assert.IsNotNull(addedCart);
            Assert.AreEqual("Address3", addedCart.DeliveryAddress);
            Assert.AreEqual(3, addedCart.Quantity);
        }

        [Test]
        public async Task UpdateCartItemAsync_ShouldUpdateCartItem()
        {
            // Arrange
            // Reinitialize the context and seed data for this specific test
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "FoodCartTestDb_" + Guid.NewGuid())
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                // Seed data
                context.Carts.AddRange(new List<Cart>
        {
            new Cart
            {
                CartID = 1,
                Quantity = 2,
                DeliveryAddress = "Address1",
                TotalCost = 20.00m,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                ItemID = 1,
                UserID = 1
            },
            new Cart
            {
                CartID = 2,
                Quantity = 1,
                DeliveryAddress = "Address2",
                TotalCost = 15.00m,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                ItemID = 2,
                UserID = 2
            }
        });

                await context.SaveChangesAsync();

                var repository = new CartRepository(context);

                // Fetch the cart item to be updated
                var existingCartItem = await repository.GetCartItemByIdAsync(1);
                Assert.IsNotNull(existingCartItem, "Existing cart item should not be null.");

                // Modify the cart item
                existingCartItem.Quantity = 3;
                existingCartItem.TotalCost = 30.00m;
                existingCartItem.UpdatedAt = DateTime.Now;

                // Act
                var updatedCartItem = await repository.UpdateCartItemAsync(existingCartItem);

                // Assert
                Assert.IsNotNull(updatedCartItem, "Updated cart item should not be null.");
                Assert.AreEqual(3, updatedCartItem.Quantity, "Quantity should be updated.");
                Assert.AreEqual(30.00m, updatedCartItem.TotalCost, "Total cost should be updated.");
                Assert.AreEqual(existingCartItem.CartID, updatedCartItem.CartID, "CartID should remain the same.");
            }
        }



        [Test]
        public async Task RemoveCartItemAsync_ShouldRemoveCartItem()
        {
            // Act
            var result = await _repository.RemoveCartItemAsync(1);

            // Assert
            Assert.IsTrue(result);
            var removedCart = await _repository.GetCartItemByIdAsync(1);
            Assert.IsNull(removedCart);
        }

        [Test]
        public async Task ClearCartAsync_ShouldClearAllCartItemsForUserId()
        {
            // Act
            await _repository.ClearCartAsync(1);

            // Assert
            var cartItems = await _repository.GetCartItemsByUserIdAsync(1);
            Assert.IsEmpty(cartItems);
        }

        [Test]
        public async Task GetCartItemsByCartIdAsync_ShouldReturnCartItemsForCartId()
        {
            // Act
            var result = await _repository.GetCartItemsByCartIdAsync(1);

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Address1", result.First().DeliveryAddress);
        }
    }
}
