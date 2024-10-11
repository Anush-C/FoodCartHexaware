using FoodCart_Hexaware.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FoodCart_Hexaware.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class DeliveryAgentsController : ControllerBase
    {
        private readonly IDeliveryAgentService _deliveryAgentService;
        private readonly ILogger<DeliveryAgentsController> _logger;

        public DeliveryAgentsController(IDeliveryAgentService deliveryAgentService, ILogger<DeliveryAgentsController> logger)
        {
            _deliveryAgentService = deliveryAgentService;
            _logger = logger;
        }

        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableAgent()
        {
            var agent = await _deliveryAgentService.GetAvailableAgentAsync();

            var availableAgent = agent.FirstOrDefault();
            if (availableAgent == null)
            {
                _logger.LogWarning("No available agents found.");
                return NotFound("No available agents at the moment.");
            }

            _logger.LogInformation($"Available agent retrieved: {availableAgent.DeliveryAgentID}");
            return Ok(agent);
        }

        [HttpGet("{agentId}")]
        public async Task<IActionResult> GetAgentById(int agentId)
        {
            var agent = await _deliveryAgentService.GetAgentByIdAsync(agentId);
            if (agent == null)
            {
                _logger.LogWarning($"Agent not found with ID: {agentId}");
                return NotFound();
            }

            _logger.LogInformation($"Retrieved agent details for ID: {agentId}");
            return Ok(agent);
        }

        [HttpGet("allagents")]
        public async Task<IActionResult> GetAllAgents()
        {
            var agents = await _deliveryAgentService.GetDeliveryAgentsAsync();
            if (agents == null || !agents.Any())
            {
                _logger.LogWarning("No delivery agents found.");
                return NotFound("No delivery agents found.");
            }

            _logger.LogInformation("Retrieved all delivery agents.");
            return Ok(agents);
        }

        [HttpPut("{id}/updateAvailability")]
        public async Task<IActionResult> UpdateAvailability(int id, [FromBody] bool isAvailable)
        {
            var result = await _deliveryAgentService.UpdateAgentAvailabilityAsync(id, isAvailable);

            if (!result)
            {
                _logger.LogWarning($"Failed to update availability for agent ID: {id}");
                return NotFound(new { message = "Delivery agent not found or unable to update availability." });
            }

            _logger.LogInformation($"Updated availability for agent ID: {id} to {isAvailable}");
            return NoContent();
        }

        [HttpPut("{orderId}/assignAgent/{agentId}")]
        public async Task<IActionResult> AssignAgent(int orderId, int agentId)
        {
            var result = await _deliveryAgentService.AssignAgentToOrderAsync(orderId, agentId);

            if (!result)
            {
                _logger.LogWarning($"Failed to assign agent ID: {agentId} to order ID: {orderId}");
                return BadRequest(new { message = "Assignment failed. Either the agent or the order was not found, or the agent is unavailable." });
            }

            _logger.LogInformation($"Assigned agent ID: {agentId} to order ID: {orderId}");
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _deliveryAgentService.GetAllOrdersAsync();
            _logger.LogInformation("Retrieved all orders.");
            return Ok(orders);
        }
    }
}
