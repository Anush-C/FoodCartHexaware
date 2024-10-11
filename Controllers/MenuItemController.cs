using FoodCart_Hexaware.Models;
using FoodCart_Hexaware.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodCart_Hexaware.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class MenuItemController : ControllerBase
    {
        private readonly IMenuItemRepository _repository;

        public MenuItemController(IMenuItemRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MenuItems>>> GetMenuItems()
        {
            try
            {
                var menuItems = await _repository.GetAllMenuItemsAsync();
                return Ok(menuItems);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error occurred while retrieving menu items: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MenuItems>> GetMenuItem(int id)
        {
            try
            {
                var menuItem = await _repository.GetMenuItemByIdAsync(id);
                if (menuItem == null)
                {
                    return NotFound($"Menu item with ID {id} not found.");
                }
                return Ok($"Here your items:{menuItem}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error occurred while retrieving menu item with ID {id}: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<MenuItems>> CreateMenuItem(MenuItems menuItem)
        {
            try
            {
                await _repository.AddMenuItemAsync(menuItem);
                return CreatedAtAction(nameof(GetMenuItem), new { id = menuItem.ItemID }, menuItem);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error occurred while creating menu item: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMenuItem(int id, MenuItems menuItem)
        {
            try
            {
                if (id != menuItem.ItemID)
                    return BadRequest("Menu item ID mismatch.");

                await _repository.UpdateMenuItemAsync(menuItem);
                return Content("Updated");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error occurred while updating menu item with ID {id}: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMenuItem(int id)
        {
            try
            {
                await _repository.DeleteMenuItemAsync(id);
                return Content("Deleted");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error occurred while deleting menu item with ID {id}: {ex.Message}");
            }
        }

        [HttpPost("{id}/availabilitystatus")]
        public async Task<IActionResult> UpdateAvailabilityStatus(int id, [FromBody] string status)
        {
            try
            {
                await _repository.UpdateAvailabilityStatusAsync(id, status);
                return Content("Updated");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error occurred while updating availability status for menu item with ID {id}: {ex.Message}");
            }
        }


    }
}
