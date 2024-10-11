using FoodCart_Hexaware.Data;
using FoodCart_Hexaware.Models;
using Microsoft.EntityFrameworkCore;
using Stripe.Climate;

namespace FoodCart_Hexaware.Services
{
    public class DeliveryAgentService : IDeliveryAgentService
    {
        private readonly ApplicationDbContext _context;

        public DeliveryAgentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task <IEnumerable<DeliveryAgent>> GetAvailableAgentAsync()
        {
            return await _context.DeliveryAgents
                .Where(da => da.IsAvailable).ToListAsync();
        }

        public async Task<DeliveryAgent> GetAgentByIdAsync(int agentId)
        {
            return await _context.DeliveryAgents.FindAsync(agentId);
        }
        public async Task<IEnumerable<DeliveryAgent>> GetDeliveryAgentsAsync()
        {
            return await _context.DeliveryAgents.ToListAsync();
        }
        public async Task<bool> UpdateAgentAvailabilityAsync(int agentId, bool isAvailable)
        {
            var agent = await _context.DeliveryAgents.FindAsync(agentId);

            if (agent == null)
            {
                return false; // Agent not found
            }

            agent.IsAvailable = isAvailable; // Update availability
            _context.Entry(agent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync(); // Save changes to the database
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false; // Handle any update concurrency issues
            }
        }

        public async Task<bool> AssignAgentToOrderAsync(int orderId, int agentId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            var agent = await _context.DeliveryAgents.FindAsync(agentId);

            if (order == null || agent == null || !agent.IsAvailable)
            {
                return false; // Either order, agent doesn't exist or agent is unavailable
            }

            // Assign the agent to the order
            order.DeliveryAgentID = agentId;

            // Optionally mark the agent as unavailable after assignment
            agent.IsAvailable = false;

            _context.Entry(order).State = EntityState.Modified;
            _context.Entry(agent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false; // Handle potential concurrency issues
            }
        }

        public async Task<IEnumerable<Orders>> GetAllOrdersAsync()
        {
            return await _context.Orders.ToListAsync();
        }

    }
}
