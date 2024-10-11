using FoodCart_Hexaware.DTO;
using FoodCart_Hexaware.DTOs;
using FoodCart_Hexaware.Models;
using FoodCart_Hexaware.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace FoodCart_Hexaware.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Hotel Owner")]
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;
        private readonly ILogger<RestaurantController> _logger; // Step 1: Add logger

        public RestaurantController(IRestaurantService restaurantService, ILogger<RestaurantController> logger)
        {
            _restaurantService = restaurantService;
            _logger = logger; // Step 1: Assign logger
        }

        [HttpGet("{restaurantId}")]
        public async Task<ActionResult<Restaurant>> GetRestaurant(int restaurantId)
        {
            _logger.LogInformation("Fetching restaurant details for restaurantId: {restaurantId}", restaurantId); // Log entry
            var restaurant = await _restaurantService.GetRestaurantByIdAsync(restaurantId);

            if (restaurant == null)
            {
                _logger.LogWarning("Restaurant not found: {restaurantId}", restaurantId); // Log warning
                return NotFound(new { message = "Restaurant not found." });
            }

            _logger.LogInformation("Successfully retrieved restaurant details for restaurantId: {restaurantId}", restaurantId); // Log success
            return Ok(restaurant);
        }

        [HttpGet("dashboard")]
        public IActionResult GetDashboard()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out var parsedUserId))
            {
                _logger.LogWarning("Unauthorized access attempt: Invalid user ID."); // Log warning
                return Unauthorized("Invalid user ID.");
            }

            try
            {
                var dashboardData = _restaurantService.GetDashboardData(parsedUserId);
                _logger.LogInformation("Successfully fetched dashboard data for userId: {userId}", parsedUserId); // Log success
                return Ok(dashboardData);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Error retrieving dashboard data for userId: {userId}", parsedUserId); // Log error
                return NotFound(ex.Message);
            }
        }

        [HttpPost("menuitem")]
        public IActionResult AddMenuItem([FromBody] MenuItemDTO menuItemDTO)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out var parsedUserId))
            {
                _logger.LogWarning("Unauthorized access attempt: Invalid user ID."); // Log warning
                return Unauthorized("Invalid user ID.");
            }

            try
            {
                _restaurantService.AddMenuItem(parsedUserId, menuItemDTO);
                _logger.LogInformation("Added menu item: {@menuItemDTO} for userId: {userId}", menuItemDTO, parsedUserId); // Log success
                return CreatedAtAction(nameof(GetMenuItems), new { restaurantId = menuItemDTO.CategoryID }, menuItemDTO);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Failed to add menu item: {@menuItemDTO} for userId: {userId}", menuItemDTO, parsedUserId); // Log error
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("menuitems/{restaurantId}")]
        public IActionResult GetMenuItems(int restaurantId)
        {
            var menuItems = _restaurantService.GetMenuItems(restaurantId);
            _logger.LogInformation("Fetched menu items for restaurantId: {restaurantId}", restaurantId); // Log success
            return Ok(menuItems);
        }

        [HttpPut("menuitem/{itemId}")]
        public IActionResult UpdateMenuItem(int itemId, [FromBody] MenuItemDTO menuItemDTO)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out var parsedUserId))
            {
                _logger.LogWarning("Unauthorized access attempt: Invalid user ID."); // Log warning
                return Unauthorized("Invalid user ID.");
            }

            try
            {
                _restaurantService.UpdateMenuItem(parsedUserId, itemId, menuItemDTO);
                _logger.LogInformation("Updated menu item {itemId} for userId: {userId}", itemId, parsedUserId); // Log success
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Failed to update menu item {itemId} for userId: {userId}", itemId, parsedUserId); // Log error
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("menuitem/{itemId}")]
        public IActionResult DeleteMenuItem(int itemId)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out var parsedUserId))
            {
                _logger.LogWarning("Unauthorized access attempt: Invalid user ID."); // Log warning
                return Unauthorized("Invalid user ID.");
            }

            try
            {
                _restaurantService.DeleteMenuItem(parsedUserId, itemId);
                _logger.LogInformation("Deleted menu item {itemId} for userId: {userId}", itemId, parsedUserId); // Log success
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Failed to delete menu item {itemId} for userId: {userId}", itemId, parsedUserId); // Log error
                return NotFound(ex.Message);
            }
        }

        [HttpGet("categories/{restaurantId}")]
        public IActionResult GetCategories(int restaurantId)
        {
            var categories = _restaurantService.GetCategories(restaurantId);
            _logger.LogInformation("Fetched categories for restaurantId: {restaurantId}", restaurantId); // Log success
            return Ok(categories);
        }

        [HttpPost("category")]
        public IActionResult AddCategory(int restaurantId, [FromBody] MenuCategoryDTO menuCategoryDTO)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out var parsedUserId))
            {
                _logger.LogWarning("Unauthorized access attempt: Invalid user ID."); // Log warning
                return Unauthorized("Invalid user ID.");
            }

            try
            {
                var category = _restaurantService.AddCategory(restaurantId, menuCategoryDTO);
                _logger.LogInformation("Added category: {@menuCategoryDTO} for restaurantId: {restaurantId}", menuCategoryDTO, restaurantId); // Log success
                return CreatedAtAction(nameof(GetCategories), new { restaurantId, categoryId = category.CategoryID }, category);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Failed to add category: {@menuCategoryDTO} for restaurantId: {restaurantId}", menuCategoryDTO, restaurantId); // Log error
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("category/{categoryId}")]
        public IActionResult UpdateCategory(int restaurantId, int categoryId, [FromBody] EditCategoryDTO editCategoryDTO)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out var parsedUserId))
            {
                _logger.LogWarning("Unauthorized access attempt: Invalid user ID."); // Log warning
                return Unauthorized("Invalid user ID.");
            }

            try
            {
                _restaurantService.UpdateCategory(restaurantId, categoryId, editCategoryDTO);
                _logger.LogInformation("Updated category {categoryId} for restaurantId: {restaurantId}", categoryId, restaurantId); // Log success
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Failed to update category {categoryId} for restaurantId: {restaurantId}", categoryId, restaurantId); // Log error
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("category/{categoryId}")]
        public IActionResult DeleteCategory(int restaurantId, int categoryId)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out var parsedUserId))
            {
                _logger.LogWarning("Unauthorized access attempt: Invalid user ID."); // Log warning
                return Unauthorized("Invalid user ID.");
            }

            try
            {
                _restaurantService.DeleteCategory(restaurantId, categoryId);
                _logger.LogInformation("Deleted category {categoryId} for restaurantId: {restaurantId}", categoryId, restaurantId); // Log success
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Failed to delete category {categoryId} for restaurantId: {restaurantId}", categoryId, restaurantId); // Log error
                return NotFound(ex.Message);
            }
        }

        [HttpGet("orders/{restaurantId}")]
        public IActionResult GetOrders(int restaurantId)
        {
            _logger.LogInformation($"Fetching orders for restaurant ID: {restaurantId}");
            var orders = _restaurantService.GetOrders(restaurantId);
            return Ok(orders);
        }

        [HttpPut("order/{orderId}")]
        public IActionResult UpdateOrderStatus(int orderId, [FromBody] UpdateRequestDTO request)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out var parsedUserId))
            {
                _logger.LogWarning("Unauthorized attempt to update order status for order ID: {OrderId}", orderId);
                return Unauthorized("Invalid user ID.");
            }

            try
            {
                _logger.LogInformation($"Updating order status for order ID: {orderId} by user ID: {parsedUserId}");
                _restaurantService.UpdateOrderStatus(parsedUserId, orderId, request.status);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, "Order not found: {OrderId}", orderId);
                return NotFound(ex.Message);
            }
        }

        [HttpGet("order/{orderId}")]
        public IActionResult GetOrderDetails(int orderId)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out var parsedUserId))
            {
                _logger.LogWarning("Unauthorized attempt to access order details for order ID: {OrderId}", orderId);
                return Unauthorized("Invalid user ID.");
            }

            try
            {
                _logger.LogInformation($"Fetching details for order ID: {orderId} by user ID: {parsedUserId}");
                var orderDetails = _restaurantService.GetOrderDetails(parsedUserId, orderId);
                return Ok(orderDetails);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, "Order details not found for order ID: {OrderId}", orderId);
                return NotFound(ex.Message);
            }
        }

        [HttpPut("menuitem/{itemId}/outofstock")]
        public IActionResult MarkMenuItemAsOutOfStock(int itemId)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out var parsedUserId))
            {
                _logger.LogWarning("Unauthorized attempt to mark item as out of stock for item ID: {ItemId}", itemId);
                return Unauthorized("Invalid user ID.");
            }

            try
            {
                _logger.LogInformation($"Marking menu item as out of stock for item ID: {itemId} by user ID: {parsedUserId}");
                _restaurantService.MarkMenuItemAsOutOfStock(parsedUserId, itemId);
                return Content("Marked Successfully");
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Failed to mark item as out of stock for item ID: {ItemId}", itemId);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while marking item as out of stock for item ID: {ItemId}", itemId);
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

    }
}
