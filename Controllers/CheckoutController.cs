using Microsoft.AspNetCore.Mvc;
using FoodCart_Hexaware.Data;
using FoodCart_Hexaware.DTO;
using FoodCart_Hexaware.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using FoodCart_Hexaware.Services;
using Microsoft.AspNetCore.Authorization;
using Stripe;
using Microsoft.Extensions.Logging;

namespace FoodCart_Hexaware.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Customer")]
    public class CheckoutController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IOrderConfirmationService _orderConfirmation;
        private readonly IStripeGatewayService _stripeGatewayService;
        private readonly ILogger<CheckoutController> _logger;

        public CheckoutController(
            ApplicationDbContext context,
            IOrderConfirmationService orderConfirmationService,
            IStripeGatewayService stripeGateway,
            ILogger<CheckoutController> logger)
        {
            _context = context;
            _orderConfirmation = orderConfirmationService;
            _stripeGatewayService = stripeGateway;
            _logger = logger;
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout([FromBody] CheckoutRequest model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for CheckoutRequest: {ModelState}", ModelState);
                return BadRequest(ModelState);
            }

            if (model.PaymentMethod.ToLower() == "card" && string.IsNullOrEmpty(model.StripePaymentMethodId))
            {
                _logger.LogWarning("Payment method ID is empty for user {UserId}", model.UserId);
                return BadRequest("Payment method ID cannot be empty.");
            }

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var cartItems = await _context.Carts
                        .Where(c => c.UserID == model.UserId)
                        .Include(c => c.MenuItems)
                        .ToListAsync();

                    if (cartItems == null || !cartItems.Any())
                    {
                        _logger.LogWarning("User {UserId} tried to checkout with an empty cart.", model.UserId);
                        return BadRequest("Your cart is empty.");
                    }

                    var restaurant = await _context.Restaurants
                        .Where(r => r.RestaurantID == model.RestaurantID)
                        .Select(r => r.RestaurantName)
                        .FirstOrDefaultAsync();

                    var totalPrice = cartItems.Sum(item => item.Quantity * item.MenuItems.ItemPrice);
                    var estimatedDeliveryTime = DateTime.Now.AddMinutes(30);

                    var order = new Orders
                    {
                        UserID = model.UserId,
                        RestaurantID = model.RestaurantID,
                        TotalPrice = totalPrice,
                        ShippingAddress = model.ShippingAddress,
                        OrderStatus = "Pending",
                        OrderTime = DateTime.Now,
                        DeliveryTime = estimatedDeliveryTime,
                        OrderItems = cartItems.Select(item => new OrderItems
                        {
                            ItemID = item.ItemID,
                            Quantity = item.Quantity,
                            Price = item.MenuItems.ItemPrice
                        }).ToList()
                    };

                    await _context.Orders.AddAsync(order);
                    await _context.SaveChangesAsync();

                    var payment = new Payment
                    {
                        Amount = totalPrice,
                        PaymentMethod = model.PaymentMethod,
                        PaymentStatus = "Pending",
                        TransDateTime = DateTime.Now,
                        OrderID = order.OrderID
                    };

                    await _context.Payments.AddAsync(payment);
                    await _context.SaveChangesAsync();

                    if (model.PaymentMethod.ToLower() == "card")
                    {
                        var paymentIntent = await _stripeGatewayService.CreatePaymentIntent(totalPrice, "usd", model.StripePaymentMethodId, "http://localhost:3001/checkout/success");

                        if (paymentIntent.Status == "succeeded")
                        {
                            payment.PaymentStatus = "Completed";
                            _context.Payments.Update(payment);
                            await _context.SaveChangesAsync();
                            _logger.LogInformation("Payment succeeded for OrderID {OrderId}", order.OrderID);
                        }
                        else if (paymentIntent.Status == "requires_action" || paymentIntent.Status == "requires_confirmation")
                        {
                            return Ok(new
                            {
                                RequiresAction = true,
                                PaymentIntentId = paymentIntent.Id,
                                ClientSecret = paymentIntent.ClientSecret
                            });
                        }
                        else
                        {
                            payment.PaymentStatus = "Failed";
                            _context.Payments.Update(payment);
                            await _context.SaveChangesAsync();
                            await transaction.RollbackAsync();
                            _logger.LogWarning("Payment failed for OrderID {OrderId} with status {Status}", order.OrderID, paymentIntent.Status);
                            return BadRequest("Payment failed.");
                        }
                    }

                    // Assign a delivery agent regardless of payment method (COD or card)
                    var deliveryAgent = await _context.DeliveryAgents
                        .Where(da => da.IsAvailable)
                        .FirstOrDefaultAsync();

                    if (deliveryAgent == null)
                    {
                        _logger.LogWarning("No available delivery agents for OrderID {OrderId}", order.OrderID);
                        return BadRequest("No available delivery agents at the moment.");
                    }

                    order.DeliveryAgentID = deliveryAgent.DeliveryAgentID;
                    deliveryAgent.IsAvailable = false;

                    _context.Orders.Update(order);
                    _context.DeliveryAgents.Update(deliveryAgent);
                    await _context.SaveChangesAsync();

                    // Commit the transaction after all changes
                    await transaction.CommitAsync();

                    // Send confirmation email regardless of payment method
                    var user = await _context.Users.FindAsync(order.UserID);
                    if (user != null)
                    {
                        await _orderConfirmation.SendOrderConfirmationEmailAsync(user.Email, order);
                    }

                    _logger.LogInformation("Order successfully placed for UserID {UserId}. OrderID: {OrderId}", model.UserId, order.OrderID);
                    return Ok(new
                    {
                        Message = $"You have successfully ordered {string.Join(", ", cartItems.Select(c => c.MenuItems.ItemName))} from {restaurant}.",
                        OrderId = order.OrderID,
                        EstimatedDeliveryTime = estimatedDeliveryTime,
                        Items = order.OrderItems.Select(item => new
                        {
                            ItemName = item.ItemID,
                            Quantity = item.Quantity,
                            Price = item.Price
                        }).ToList()
                    });
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError(ex, "An error occurred while processing checkout for UserID {UserId}", model.UserId);
                    return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while processing your request: {ex.Message}");
                }
            }
        }

        [HttpGet("checkout/success")]
        public IActionResult CheckoutSuccess()
        {
            return Ok("Payment succeeded!");
        }
    }
}
