using FoodCart_Hexaware.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodCart_Hexaware.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrder(int orderId)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);
            if (order == null)
                return NotFound();

            return Ok(order);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetOrdersByUser(int userId)
        {
            var orders = await _orderService.GetOrdersByUserIdAsync(userId);
            return Ok(orders);
        }

        [HttpPost("assign-delivery/{orderId}")]
        public async Task<IActionResult> AssignDeliveryAgent(int orderId, [FromBody] int agentId)
        {
            var success = await _orderService.AssignDeliveryAgentAsync(orderId, agentId);
            if (!success)
                return BadRequest("Assignment failed.");

            return Ok("Delivery agent assigned successfully.");
        }

        [HttpGet("confirmation/{orderId}")]
        public async Task<IActionResult> GetOrderConfirmation(int orderId)
        {
            var confirmation = await _orderService.GetOrderConfirmationAsync(orderId);
            if (confirmation == null)
                return NotFound("Order not found");

            return Ok(confirmation);
        }

        [HttpPost("cancel/{orderId}")]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            var result = await _orderService.CancelOrderAsync(orderId);
            if (!result)
                return NotFound("Order not found or cannot be cancelled.");

            return Ok("Order has been successfully cancelled.");
        }

    }
}
