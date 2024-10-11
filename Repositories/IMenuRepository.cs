using FoodCart_Hexaware.DTO;
using FoodCart_Hexaware.Models;

namespace FoodCart_Hexaware.Repositories
{
    public interface IMenuRepository
    {
        Task<IEnumerable<MenuItems>> GetMenuItem();
        Task<MenuItems?> GetMenuItembyId(int id);
        Task<IEnumerable<MenuItems>> GetMenuItembyName(string name);
        Task<IEnumerable<MenuItems>> GetMenuItembyCategory(string categoryname);
        Task<IEnumerable<MenuItems>> GetMenuItemsbyCuisine(string cuisine);
        Task<IEnumerable<MenuItems>> GetMenuItemsbyAvailability();
        Task<IEnumerable<MenuItems>> GetMenuItemsbyPrice(int maxprice, int minprice);
        Task<IEnumerable<MenuItems>> GetMenuItemsbySearch(string query);

        Task<IEnumerable<MenuItems>> GetMenuItemsByFilters(string? type, string? category, decimal? minprice, decimal? maxprice, string? cuisine);

        Task<IEnumerable<Restaurant>> GetMenuItemsByLocation(string location);

        Task LinkMenuItemToRestaurant(int restaurantId, int menuItemId);

        Task<IEnumerable<MenuItems>> GetMenuItemsByRestaurantId(int restaurantId);

        Task<ItemCartDTO> GetMenuItemNamebyItemId(int id);
    }
}
