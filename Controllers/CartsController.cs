using FoodCart_Hexaware.DTO;
using FoodCart_Hexaware.Models;
using FoodCart_Hexaware.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging; // Include the logging namespace

namespace FoodCart_Hexaware.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Customer")]
    public class CartsController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;
        private readonly ILogger<CartsController> _logger; // Logger instance

        public CartsController(ICartRepository cartRepository, ILogger<CartsController> logger)
        {
            _cartRepository = cartRepository;
            _logger = logger; // Initialize the logger
        }

        [HttpGet("AllCarts")]
        public async Task<ActionResult<IEnumerable<CartDTO>>> GetCarts()
        {
            try
            {
                _logger.LogInformation("Retrieving all cart items.");
                var carts = await _cartRepository.GetAllCartsAsync();

                if (carts == null || !carts.Any())
                {
                    _logger.LogWarning("No cart items found.");
                    return NotFound(new { message = "No cart items found" });
                }

                _logger.LogInformation("Successfully retrieved all cart items.");
                return Ok(carts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the cart items.");
                return StatusCode(500, new { message = "An error occurred while retrieving the cart items.", details = ex.Message });
            }
        }

        [HttpGet("UserCart/{userId}")]
        public async Task<IActionResult> GetCartItemsByUserId(int userId)
        {
            _logger.LogInformation($"Retrieving cart items for user ID: {userId}");
            var cartItems = await _cartRepository.GetCartItemsByUserIdAsync(userId);

            if (cartItems == null || !cartItems.Any())
            {
                _logger.LogWarning($"No items found in the cart for user ID: {userId}");
                return NotFound(new { message = "No items found in the cart for this user." });
            }

            _logger.LogInformation($"Successfully retrieved cart items for user ID: {userId}");
            return Ok(cartItems);
        }

        [HttpPost("AddToCart")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartDTO addToCartDto)
        {
            try
            {
                _logger.LogInformation($"Adding item with ID: {addToCartDto.ItemID} to cart for user ID: {addToCartDto.UserID}");
                var menuItem = await _cartRepository.GetMenuItemByIdAsync(addToCartDto.ItemID);
                if (menuItem == null)
                {
                    _logger.LogWarning($"Menu item with ID: {addToCartDto.ItemID} not found.");
                    return NotFound(new { message = "Menu item not found" });
                }

                decimal totalCost = menuItem.ItemPrice * addToCartDto.Quantity;

                var cart = new Cart
                {
                    UserID = addToCartDto.UserID,
                    ItemID = addToCartDto.ItemID,
                    Quantity = addToCartDto.Quantity,
                    TotalCost = totalCost,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                var newCart = await _cartRepository.AddToCartAsync(addToCartDto);
                _logger.LogInformation($"Item added to cart successfully. Cart ID: {newCart.CartID}");
                return Ok(new { message = "Item added to cart successfully", newCart.CartID });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding the item to the cart.");
                return StatusCode(500, new { message = "An error occurred while adding the item to the cart.", details = ex.Message });
            }
        }

        [HttpPut("UpdateCart")]
        public async Task<IActionResult> UpdateCart([FromBody] UpdateCartDTO updateCartDto)
        {
            try
            {
                if (updateCartDto.Quantity <= 0)
                {
                    _logger.LogWarning("Quantity must be greater than zero.");
                    return BadRequest(new { message = "Quantity must be greater than zero." });
                }

                var existingCartItem = await _cartRepository.GetCartItemByIdAsync(updateCartDto.CartID);
                if (existingCartItem == null)
                {
                    _logger.LogWarning($"Cart item with ID {updateCartDto.CartID} not found.");
                    return NotFound(new { message = $"Cart item with ID {updateCartDto.CartID} not found." });
                }

                existingCartItem.Quantity = updateCartDto.Quantity;
                var menuItem = await _cartRepository.GetMenuItemByIdAsync(existingCartItem.ItemID);
                if (menuItem == null)
                {
                    _logger.LogWarning($"Menu item with ID {existingCartItem.ItemID} not found.");
                    return NotFound(new { message = $"Menu item with ID {existingCartItem.ItemID} not found." });
                }

                existingCartItem.TotalCost = existingCartItem.Quantity * menuItem.ItemPrice;
                existingCartItem.UpdatedAt = DateTime.Now;

                await _cartRepository.UpdateCartItemAsync(existingCartItem);
                _logger.LogInformation($"Cart item updated successfully. Cart ID: {existingCartItem.CartID}");
                return Ok(new { message = "Cart item updated successfully", cartItem = existingCartItem });
            }
            catch (KeyNotFoundException knfEx)
            {
                _logger.LogWarning(knfEx, "Cart item not found.");
                return NotFound(new { message = knfEx.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the cart item.");
                return StatusCode(500, new { message = "An error occurred while updating the cart item.", details = ex.Message });
            }
        }

        [HttpDelete("RemoveCartItem/{cartId}")]
        public async Task<IActionResult> RemoveCartItem(int cartId)
        {
            try
            {
                _logger.LogInformation($"Removing cart item with ID: {cartId}");
                var result = await _cartRepository.RemoveCartItemAsync(cartId);
                if (result)
                {
                    _logger.LogInformation("Cart item removed successfully.");
                    return Content("Cart item removed successfully");
                }
                else
                {
                    _logger.LogWarning("Cart item not found.");
                    return NotFound(new { message = "Cart item not found" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while removing the cart item.");
                return StatusCode(500, new { message = "An error occurred while removing the cart item.", details = ex.Message });
            }
        }

        [HttpGet("GetCartItems/{cartId}")]
        public async Task<IActionResult> GetCartItemsByCartId(int cartId)
        {
            try
            {
                _logger.LogInformation($"Retrieving cart items for cart ID: {cartId}");
                var cartItems = await _cartRepository.GetCartItemsByCartIdAsync(cartId);

                if (cartItems == null || !cartItems.Any())
                {
                    _logger.LogWarning($"No items found for cart ID: {cartId}");
                    return NotFound(new { message = "No items found for the given cart ID." });
                }

                _logger.LogInformation($"Successfully retrieved cart items for cart ID: {cartId}");
                return Ok(cartItems);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the cart items.");
                return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
            }
        }
    }
}
