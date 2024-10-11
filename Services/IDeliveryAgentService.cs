using FoodCart_Hexaware.Models;

namespace FoodCart_Hexaware.Services
{
    public interface IDeliveryAgentService
    {
        Task<IEnumerable<DeliveryAgent>> GetAvailableAgentAsync();
        Task<DeliveryAgent> GetAgentByIdAsync(int agentId);

        Task<IEnumerable<DeliveryAgent>> GetDeliveryAgentsAsync();

        Task<bool> UpdateAgentAvailabilityAsync(int agentId, bool isAvailable);

        Task<bool> AssignAgentToOrderAsync(int orderId, int agentId);

        Task<IEnumerable<Orders>> GetAllOrdersAsync();
    }
}
