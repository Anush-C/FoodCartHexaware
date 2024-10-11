using FoodCart_Hexaware.DTO;
using FoodCart_Hexaware.Models;

namespace FoodCart_Hexaware.Services
{
    public interface IOrderService
    {
        Task<Orders> GetOrderByIdAsync(int orderId);
        Task<IEnumerable<Orders>> GetOrdersByUserIdAsync(int userId);
        Task<IEnumerable<OrderItems>> GetOrderItemsAsync(int orderId);
        Task<bool> AssignDeliveryAgentAsync(int orderId, int agentId);

        // New Method for order confirmation
        Task<OrderConfirmationDTO> GetOrderConfirmationAsync(int orderId);
        Task<bool> CancelOrderAsync(int orderId);
    }
}
