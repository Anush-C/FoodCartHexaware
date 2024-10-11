using FoodCart_Hexaware.Models;

namespace FoodCart_Hexaware.Repositories
{
    public interface IMenuItemRepository
    {
        Task<IEnumerable<MenuItems>> GetAllMenuItemsAsync();
        Task<MenuItems> GetMenuItemByIdAsync(int id);
        Task AddMenuItemAsync(MenuItems menuItem);
        Task UpdateMenuItemAsync(MenuItems menuItem);
        Task DeleteMenuItemAsync(int id);
        Task UpdateAvailabilityStatusAsync(int id, string status);
        

    }
}
