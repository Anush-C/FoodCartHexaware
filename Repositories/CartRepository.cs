using FoodCart_Hexaware.Data;
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

        public async Task<IEnumerable<Cart>> GetAllCartItemsAsync()
        {
            return await _context.Carts.ToListAsync();
        }

        public async Task<Cart> AddToCartAsync(Cart cart)
        {
            await _context.Carts.AddAsync(cart);
            await _context.SaveChangesAsync();
            return cart;
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

