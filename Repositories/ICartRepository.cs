using FoodCart_Hexaware.DTO;
using FoodCart_Hexaware.Models;

namespace FoodCart_Hexaware.Repositories
{
    public interface ICartRepository
    {
        Task<Cart> AddToCartAsync(AddToCartDTO addToCartDto);
        Task<Cart> UpdateCartItemAsync(Cart cart);
        Task<bool> RemoveCartItemAsync(int cartId);
        Task<MenuItems> GetMenuItemByIdAsync(int itemId);
        Task<Cart> GetCartItemByIdAsync(int cartId);

        Task<List<CartDTO>> GetAllCartsAsync();

        Task<IEnumerable<Cart>> GetCartItemsByUserIdAsync(int userId);
        Task ClearCartAsync(int cartId);

        Task<List<Cart>> GetCartItemsByCartIdAsync(int cartId);
    }
}
