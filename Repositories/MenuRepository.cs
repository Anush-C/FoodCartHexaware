using FoodCart_Hexaware.Data;
using FoodCart_Hexaware.DTO;
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
            if (string.IsNullOrWhiteSpace(query))
            {
                return Enumerable.Empty<MenuItems>(); 
            }

            query = query.Trim().ToLower(); 

            return await _context.Menus
                .Include(mi => mi.Restaurants)
                .Where(mi => mi.ItemName.ToLower().Contains(query)) 
                .ToListAsync();
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
        public async Task LinkMenuItemToRestaurant(int restaurantId, int menuItemId)
        {
            // Retrieve the restaurant including its associated menu items
            var restaurant = await _context.Restaurants
                                           .Include(r => r.MenuItems)
                                           .FirstOrDefaultAsync(r => r.RestaurantID == restaurantId);

            if (restaurant == null)
            {
                throw new ArgumentException("Restaurant not found.");
            }

            // Retrieve the menu item
            var menuItem = await _context.Menus
                                         .FirstOrDefaultAsync(mi => mi.ItemID == menuItemId);

            if (menuItem == null)
            {
                throw new ArgumentException("Menu item not found.");
            }

            // Check if the relationship already exists
            if (restaurant.MenuItems.Contains(menuItem))
            {
                throw new ArgumentException("Menu item is already linked to the restaurant.");
            }

            // Add the menu item to the restaurant's menu items collection
            restaurant.MenuItems.Add(menuItem);

            // Save changes to the database (EF Core will handle updating the junction table)
            await _context.SaveChangesAsync();
        }


        public async Task<IEnumerable<Restaurant>> GetMenuItemsByLocation(string location)
        {

           
            var res =   await _context.Restaurants.Include(mi=>mi.MenuItems).Where(mi=>mi.RestaurantAddress.Contains(location)).ToListAsync();

            if (res == null || !res.Any())
            {
                throw new ArgumentException("No Available Restaurants for your entered location. Please try again");
            }
            return res;
        }
        public async Task<IEnumerable<MenuItems>> GetMenuItemsByRestaurantId(int restaurantId)
        {
            return await _context.Menus
                .Include(mi => mi.Restaurants) // Assuming there’s a navigation property
                .Where(mi => mi.Restaurants.Any(r => r.RestaurantID == restaurantId))
                .ToListAsync();
        }
        public async Task<ItemCartDTO> GetMenuItemNamebyItemId(int id)
        {
            // Fetch the menu item based on the given ID
            var menuItem = await _context.Menus
                .Where(m => m.ItemID == id) // Adjust the property name based on your Menu entity
                .Select(m => new ItemCartDTO
                {
                    ItemName = m.ItemName // Assuming 'Name' is the property holding the item's name
                })
                .FirstOrDefaultAsync();

            return menuItem;
        }


    }
}
