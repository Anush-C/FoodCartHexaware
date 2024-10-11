using FoodCart_Hexaware.DTO;
using FoodCart_Hexaware.Models;
using FoodCart_Hexaware.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FoodCart_Hexaware.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IMenuItemRepository _menuItemrepository;
        private readonly IMenuCategoryRepository _menuCategoryRepository;
        private readonly ILogger<AdminController> _logger; 

        public AdminController(
            IUserRepository userRepository,
            IRestaurantRepository restaurantRepository,
            IMenuItemRepository menuItemRepository,
            IMenuCategoryRepository menuCategoryRepository,
            ILogger<AdminController> logger) // Inject logger
        {
            _userRepository = userRepository;
            _restaurantRepository = restaurantRepository;
            _menuItemrepository = menuItemRepository;
            _menuCategoryRepository = menuCategoryRepository;
            _logger = logger; // Assign logger
        }

        [HttpGet("users")]
        public async Task<ActionResult<IEnumerable<Users>>> GetUsers()
        {
            try
            {
                _logger.LogInformation("Fetching all users");
                var users = await _userRepository.GetAllUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while fetching users: {ex.Message}");
                return StatusCode(500, $"Error occurred while fetching users: {ex.Message}");
            }
        }

        [HttpPost("users")]
        public async Task<ActionResult<Users>> CreateUser(CreateUserDTO createUserDto)
        {
            try
            {
                _logger.LogInformation("Creating a new user");
                var user = await _userRepository.CreateUserAsync(createUserDto);
                return CreatedAtAction(nameof(GetUsers), new { id = user.UserID }, user);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning($"Validation error while creating user: {ex.Message}");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while creating user: {ex.Message}");
                return StatusCode(500, $"Error occurred while creating user: {ex.Message}");
            }
        }

        [HttpPut("users/{id}")]
        public async Task<IActionResult> UpdateUser(int id, UpdateUserDTO updateUserDTO)
        {
            if (id != updateUserDTO.UserID)
            {
                return BadRequest("User ID mismatch");
            }

            try
            {
                _logger.LogInformation($"Updating user with ID {id}");
                await _userRepository.UpdateUserAsync(updateUserDTO);
                return Content("Updated");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while updating user: {ex.Message}");
                return StatusCode(500, $"Error occurred while updating user: {ex.Message}");
            }
        }

       

        [HttpGet("restaurants")]
        public async Task<ActionResult<IEnumerable<Restaurant>>> GetRestaurants()
        {
            try
            {
                _logger.LogInformation("Fetching all restaurants");
                var restaurants = await _restaurantRepository.GetAllRestaurantsAsync();
                return Ok(restaurants);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while fetching restaurants: {ex.Message}");
                return StatusCode(500, $"Error occurred while fetching restaurants: {ex.Message}");
            }
        }

        [HttpPost("restaurants")]
        public async Task<ActionResult<Restaurant>> CreateRestaurant(Restaurant restaurant)
        {
            try
            {
                _logger.LogInformation("Creating a new restaurant");
                await _restaurantRepository.AddRestaurantAsync(restaurant);
                return CreatedAtAction(nameof(GetRestaurants), new { id = restaurant.RestaurantID }, restaurant);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while creating restaurant: {ex.Message}");
                return StatusCode(500, $"Error occurred while creating restaurant: {ex.Message}");
            }
        }

        [HttpPut("restaurants/{id}")]
        public async Task<IActionResult> UpdateRestaurant(int id, Restaurant restaurant)
        {
            if (id != restaurant.RestaurantID)
            {
                return BadRequest("Restaurant ID mismatch");
            }

            try
            {
                _logger.LogInformation($"Updating restaurant with ID {id}");
                await _restaurantRepository.UpdateRestaurantAsync(restaurant);
                return Content("Updated");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while updating restaurant: {ex.Message}");
                return StatusCode(500, $"Error occurred while updating restaurant: {ex.Message}");
            }
        }

        [HttpDelete("restaurants/{id}")]
        public async Task<IActionResult> DeleteRestaurant(int id)
        {
            try
            {
                _logger.LogInformation($"Deleting restaurant with ID {id}");
                await _restaurantRepository.DeleteRestaurantAsync(id);
                return Content("Deleted");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while deleting restaurant: {ex.Message}");
                return StatusCode(500, $"Error occurred while deleting restaurant: {ex.Message}");
            }
        }

        [HttpGet("items")]
        public async Task<ActionResult<IEnumerable<MenuItems>>> GetMenuItems()
        {
            try
            {
                _logger.LogInformation("Fetching all menu items");
                var menuItems = await _menuItemrepository.GetAllMenuItemsAsync();
                return Ok(menuItems);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while retrieving menu items: {ex.Message}");
                return StatusCode(500, $"Error occurred while retrieving menu items: {ex.Message}");
            }
        }

        [HttpPost("menuitems")]
        public async Task<ActionResult<MenuItems>> CreateMenuItem(MenuItems menuItem)
        {
            try
            {
                _logger.LogInformation("Creating a new menu item");
                await _menuItemrepository.AddMenuItemAsync(menuItem);
                return CreatedAtAction(nameof(GetMenuItems), new { id = menuItem.ItemID }, menuItem);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while creating menu item: {ex.Message}");
                return StatusCode(500, $"Error occurred while creating menu item: {ex.Message}");
            }
        }

        [HttpPut("menuitems/{id}")]
        public async Task<IActionResult> UpdateMenuItem(int id, MenuItems menuItem)
        {
            if (id != menuItem.ItemID)
            {
                return BadRequest("Menu Item ID mismatch");
            }

            try
            {
                _logger.LogInformation($"Updating menu item with ID {id}");
                await _menuItemrepository.UpdateMenuItemAsync(menuItem);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while updating menu item: {ex.Message}");
                return StatusCode(500, $"Error occurred while updating menu item: {ex.Message}");
            }
        }

        [HttpDelete("menuitems/{id}")]
        public async Task<IActionResult> DeleteMenuItem(int id)
        {
            try
            {
                _logger.LogInformation($"Deleting menu item with ID {id}");
                await _menuItemrepository.DeleteMenuItemAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while deleting menu item: {ex.Message}");
                return StatusCode(500, $"Error occurred while deleting menu item: {ex.Message}");
            }
        }

        [HttpGet("AllCategories")]
        public async Task<ActionResult<IEnumerable<MenuCategory>>> GetCategories()
        {
            try
            {
                _logger.LogInformation("Fetching all categories");
                var categories = await _menuCategoryRepository.GetAllCategoriesAsync();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while retrieving categories: {ex.Message}");
                return StatusCode(500, $"Error occurred while retrieving categories: {ex.Message}");
            }
        }

        [HttpPost("categories")]
        public async Task<ActionResult<MenuCategory>> CreateCategory(MenuCategory category)
        {
            try
            {
                _logger.LogInformation("Creating a new menu category");
                await _menuCategoryRepository.AddCategoryAsync(category);
                return CreatedAtAction(nameof(GetCategories), new { id = category.CategoryID }, category);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while creating menu category: {ex.Message}");
                return StatusCode(500, $"Error occurred while creating menu category: {ex.Message}");
            }
        }

        [HttpPut("categories/{id}")]
        public async Task<IActionResult> UpdateCategory(int id, MenuCategory category)
        {
            if (id != category.CategoryID)
            {
                return BadRequest("Menu Category ID mismatch");
            }

            try
            {
                _logger.LogInformation($"Updating menu category with ID {id}");
                await _menuCategoryRepository.UpdateCategoryAsync(category);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while updating menu category: {ex.Message}");
                return StatusCode(500, $"Error occurred while updating menu category: {ex.Message}");
            }
        }

        [HttpDelete("categories/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                _logger.LogInformation($"Deleting menu category with ID {id}");
                await _menuCategoryRepository.DeleteCategoryAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while deleting menu category: {ex.Message}");
                return StatusCode(500, $"Error occurred while deleting menu category: {ex.Message}");
            }
        }
    }
}
