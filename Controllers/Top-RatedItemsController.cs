using FoodCart_Hexaware.Data;
using FoodCart_Hexaware.DTO;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodCart_Hexaware.Controllers
{

    [Route("api/[controller]")]
    [ApiController]


    public class Top_RatedItemsController
    {
        private readonly ApplicationDbContext _context;

        public Top_RatedItemsController(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
             
        }

        [HttpGet]
        [Route("byTopRatedItems")]

        public async Task<IEnumerable<UIMenuDTO>> GetTopRatedMenuItemsWithDetails(int topN)
        {
            try
            {
                var topRatedItems = await _context.Menus
                    .Include(mi => mi.Restaurants)
                    .OrderByDescending(mi => mi.Rating) // Sort by rating
                    .Take(topN) // Get the top N rated items
                    .Select(mi => new UIMenuDTO // Project the result to a custom DTO
                    {
                        MenuItemName = mi.ItemName,
                        Rating = mi.Rating,
                        Items = mi.Restaurants.Select(mi => new UIRestaurantDTO
                        {
                            RestaurantName = mi.RestaurantName,
                            RestaurantAddress = mi.RestaurantAddress
                        }).ToList()
                    })
                    .ToListAsync();

                return topRatedItems;
            }
            catch (Exception ex)
            {
                throw new Exception();
            }

            
        }

    }
}
