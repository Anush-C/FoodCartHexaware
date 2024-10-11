using FoodCart_Hexaware.Data;
using FoodCart_Hexaware.DTO;
using FoodCart_Hexaware.DTOs;
using FoodCart_Hexaware.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodCart_Hexaware.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;

        }

        public async Task<Orders> GetOrderByIdAsync(int orderId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.MenuItems)
                .Include(o => o.DeliveryAgent)
                .FirstOrDefaultAsync(o => o.OrderID == orderId);
        }

        public async Task<IEnumerable<Orders>> GetOrdersByUserIdAsync(int userId)
        {
            return await _context.Orders
                .Where(o => o.UserID == userId)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.MenuItems)
                .ToListAsync();
        }

        public async Task<IEnumerable<OrderItems>> GetOrderItemsAsync(int orderId)
        {
            return await _context.OrderItems
                .Where(oi => oi.OrderID == orderId)
                .Include(oi => oi.MenuItems)
                .ToListAsync();
        }

        public async Task<bool> AssignDeliveryAgentAsync(int orderId, int agentId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
                return false;

            var agent = await _context.DeliveryAgents.FindAsync(agentId);
            if (agent == null || !agent.IsAvailable)
                return false;

            order.DeliveryAgentID = agentId;
            order.OrderStatus = "Assigned";
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<OrderConfirmationDTO> GetOrderConfirmationAsync(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.MenuItems)
                .Include(o => o.Restaurant)
                .Include(o => o.DeliveryAgent)
                .FirstOrDefaultAsync(o => o.OrderID == orderId);

            if (order == null)
                return null;

            // Map the order details to DTO
            var confirmationDto = new OrderConfirmationDTO
            {
                OrderID = order.OrderID,
                RestaurantName = order.Restaurant?.RestaurantName,
                DeliveryAgentName = order.DeliveryAgent?.Name,
                DeliveryAgentPhone = order.DeliveryAgent?.PhoneNumber,
                PaymentMethod = "Stripe", // Assuming RazorPay is the method, modify as needed.
                DeliveryTime = order.DeliveryTime,
                Items = order.OrderItems.Select(oi => new OrderItemsDTO
                {
                    ItemName = oi.MenuItems?.ItemName,
                    Quantity = oi.Quantity,
                    Price = oi.Price,
                }).ToList()
            };

            return confirmationDto;
        }

        public async Task<bool> CancelOrderAsync(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
                return false;

            // Check if the order can be cancelled based on its status
            if (order.OrderStatus == "Completed" || order.OrderStatus == "Cancelled")
                return false; // Can't cancel already completed or cancelled orders

            order.OrderStatus = "Cancelled";
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

            return true;
        }

    }
}



