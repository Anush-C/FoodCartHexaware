using FoodCart_Hexaware.Data;
using FoodCart_Hexaware.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodCart_Hexaware.Repositories
{
    public class RestaurantRepository : IRestaurantRepository
    {
        private readonly ApplicationDbContext _context;

        public RestaurantRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Restaurant>> GetAllRestaurantsAsync()
        {
            return await _context.Restaurants.ToListAsync();
        }

        public async Task<Restaurant> GetRestaurantByIdAsync(int id)
        {
            return await _context.Restaurants.FindAsync(id);
        }

        public async Task AddRestaurantAsync(Restaurant restaurant)
        {
            _context.Restaurants.Add(restaurant);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRestaurantAsync(Restaurant restaurant)
        {
            var existingRestaurant = await _context.Restaurants
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.RestaurantID == restaurant.RestaurantID);

            if (existingRestaurant != null)
            {
                _context.Entry(restaurant).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
        }


        public async Task DeleteRestaurantAsync(int id)
        {
            var restaurant = await _context.Restaurants.FindAsync(id);
            if (restaurant != null)
            {
                _context.Restaurants.Remove(restaurant);
                await _context.SaveChangesAsync();
            }
        }
    }
}
