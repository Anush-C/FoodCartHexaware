using FoodCart_Hexaware.Data;
using FoodCart_Hexaware.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodCart_Hexaware.Repositories
{
    public class MenuRepository : IMenuRepository
    {
        private readonly ApplicationDbContext _context;
        public MenuRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MenuItems>> GetMenuItem()
        {
            return await _context.Menus.Include(mi => mi.Restaurants).ToListAsync();
        }


        public async Task<MenuItems?> GetMenuItembyId(int id)
        {
            return await _context.Menus.Include(mi => mi.Restaurants).FirstOrDefaultAsync(mi => mi.ItemID == id);
        }

        public async Task<IEnumerable<MenuItems>> GetMenuItembyName(string name)
        {
            return await _context.Menus.Include(mi => mi.Restaurants).Where(mi => mi.ItemName.Trim().ToLower() == name.Trim().ToLower()).ToListAsync();
        }

        public async Task<IEnumerable<MenuItems>> GetMenuItembyCategory(string categoryname)
        {
            var category = await _context.MenuCategories.FirstOrDefaultAsync(mi => mi.CategoryName.ToLower() == categoryname.ToLower());

            if (category == null)
            {
                return Enumerable.Empty<MenuItems>();
            }

            return await _context.Menus.Include(mi => mi.Restaurants).Where(mi => mi.CategoryID == category.CategoryID).ToListAsync();
        }

        public async Task<IEnumerable<MenuItems>> GetMenuItemsbyCuisine(string cuisine)
        {
            return await _context.Menus.Include(mi => mi.Restaurants).Where(mi => mi.CuisineType.ToLower() == cuisine.ToLower()).ToListAsync();
        }

        public async Task<IEnumerable<MenuItems>> GetMenuItemsbyAvailability()
        {
            return await _context.Menus.Include(mi => mi.Restaurants).Where(mi => mi.AvailabilityStatus == "Available").ToListAsync();
        }

        public async Task<IEnumerable<MenuItems>> GetMenuItemsbyPrice(int maxprice, int minprice)
        {
            return await _context.Menus.Include(mi => mi.Restaurants).Where(mi => mi.ItemPrice > minprice && mi.ItemPrice <= maxprice).ToListAsync();
        }

        public async Task<IEnumerable<MenuItems>> GetMenuItemsbySearch(string query)
        {
            return await _context.Menus.Include(mi => mi.Restaurants).Where(mi => mi.ItemName.Trim().ToLower() == query.Trim().ToLower()).ToListAsync();

        }
        public async Task<IEnumerable<MenuItems>> GetMenuItemsByFilters(string? type, string? category, decimal? minprice, decimal? maxprice, string? cuisine)
        {
            var list = _context.Menus.Include(mi => mi.Restaurants).AsQueryable();
            if (!string.IsNullOrEmpty(type))
            {
                list = list.Where(mi => mi.DietaryInfo.ToLower() == type.ToLower());
            }
            if (!string.IsNullOrEmpty(category))
            {
                var cat = await _context.MenuCategories.FirstOrDefaultAsync(mi => mi.CategoryName == category);
                if (cat != null)
                {
                    list = list.Where(mi => mi.CategoryID == cat.CategoryID);
                }
            }
            if (!string.IsNullOrEmpty(cuisine))
            {
                list = list.Where(mi => mi.CuisineType.ToLower() == cuisine.ToLower());
            }
            if (minprice.HasValue)
            {
                list = list.Where(mi => mi.ItemPrice > minprice);
            }
            if (maxprice.HasValue)
            {
                list = list.Where(mi => mi.ItemPrice <= maxprice);
            }

            return await list.ToListAsync();
        }
        public async Task<MenuItems> LinkMenuItemToRestaurant(int menuitemId, int restaurantId)
        {
            var menu = await _context.Menus.Include(mi => mi.Restaurants)
                                           .FirstOrDefaultAsync(mi => mi.ItemID == menuitemId);

            var restaurant = await _context.Restaurants.FirstOrDefaultAsync(r => r.RestaurantID == restaurantId);

            if (menu == null || restaurant == null)
            {
                throw new ArgumentException("Menu item or restaurant not found.");
            }

            if (!menu.Restaurants.Contains(restaurant))
            {
                menu.Restaurants.Add(restaurant);
                await _context.SaveChangesAsync();
            }

            return menu;
        }



    }
}
