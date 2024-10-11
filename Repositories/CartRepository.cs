using FoodCart_Hexaware.Data;
using FoodCart_Hexaware.DTO;
using FoodCart_Hexaware.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodCart_Hexaware.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _context;

        public CartRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<CartDTO>> GetAllCartsAsync()
        {
            return await _context.Carts
                .Select(c => new CartDTO
                {
                    CartID = c.CartID,
                    Quantity = c.Quantity,
                    DeliveryAddress = c.DeliveryAddress,
                    TotalCost = c.Quantity * c.MenuItems.ItemPrice, // Calculate total cost based on quantity and item price
                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt,
                    ItemID = c.ItemID,
                    UserID = c.UserID,
                    ItemName = c.MenuItems.ItemName, // Assuming MenuItems has ItemName property
                    RestaurantName = c.MenuItems.Restaurants.FirstOrDefault().RestaurantName // Get the first restaurant name
                })
                .ToListAsync();
        }
      



        public async Task<Cart> AddToCartAsync(AddToCartDTO addToCartDto)
        {
            // Fetch the menu item including its associated restaurants
            var menuItem = await _context.Menus
                .Include(mi => mi.Restaurants)
                .FirstOrDefaultAsync(mi => mi.ItemID == addToCartDto.ItemID);

            if (menuItem == null)
            {
                throw new Exception("Menu item not found.");
            }

            // Check if the menu item belongs to the specified restaurant
            var restaurantExists = menuItem.Restaurants.Any(r => r.RestaurantID == addToCartDto.RestaurantID);

            if (!restaurantExists)
            {
                throw new Exception("Menu item does not belong to the specified restaurant.");
            }

            // Check if the item already exists in the user's cart
            var existingCartItem = await _context.Carts
                .FirstOrDefaultAsync(c => c.UserID == addToCartDto.UserID && c.ItemID == addToCartDto.ItemID);

            if (existingCartItem != null)
            {
                // If the item already exists, update the quantity
                existingCartItem.Quantity += addToCartDto.Quantity;
                _context.Carts.Update(existingCartItem);
            }
            else
            {
                // Create a new Cart object with the data from the DTO
                var cart = new Cart
                {
                    UserID = addToCartDto.UserID,
                    ItemID = addToCartDto.ItemID,
                    Quantity = addToCartDto.Quantity
                  
                };

                // Add the new cart item to the database
                await _context.Carts.AddAsync(cart);
            }

            // Save changes to the database
            await _context.SaveChangesAsync();

            return existingCartItem ?? await _context.Carts.FirstOrDefaultAsync(c => c.UserID == addToCartDto.UserID && c.ItemID == addToCartDto.ItemID);
        }


        public async Task<Cart> UpdateCartItemAsync(Cart cart)
        {
            var existingCart = await _context.Carts.FindAsync(cart.CartID);
            if (existingCart == null)
            {
                throw new KeyNotFoundException($"Cart item with ID {cart.CartID} not found.");
            }

            existingCart.Quantity = cart.Quantity;
            existingCart.TotalCost = cart.TotalCost;
            existingCart.UpdatedAt = cart.UpdatedAt;

            await _context.SaveChangesAsync();
            return existingCart;
        }


        public async Task<bool> RemoveCartItemAsync(int cartId)
        {
            var cartItem = await _context.Carts.FirstOrDefaultAsync(c => c.CartID == cartId);

            if (cartItem == null)
            {
                return false;
            }

            _context.Carts.Remove(cartItem);
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<MenuItems> GetMenuItemByIdAsync(int itemId)
        {
            return await _context.Menus.FirstOrDefaultAsync(mi => mi.ItemID == itemId);
        }

        public async Task<Cart> GetCartItemByIdAsync(int cartId)
        {
            return await _context.Carts
                .FirstOrDefaultAsync(c => c.CartID == cartId);
        }


        public async Task<IEnumerable<Cart>> GetCartItemsByUserIdAsync(int userId)
        {
            return await _context.Carts
                .Where(c => c.UserID == userId)
                .ToListAsync();
        }

        public async Task ClearCartAsync(int cartId)
        {
            var cartItems = await _context.Carts
                .Where(c => c.CartID == cartId)
                .ToListAsync();

            _context.Carts.RemoveRange(cartItems);
            await _context.SaveChangesAsync();
        }



        public async Task<List<Cart>> GetCartItemsByCartIdAsync(int cartId)
        {
            try
            {
                var cartItems = await _context.Carts
                    .Where(c => c.CartID == cartId)
                    .ToListAsync();

                return cartItems;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving cart items: {ex.Message}", ex);
            }
        }
    }
}

