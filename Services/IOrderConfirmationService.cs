using FoodCart_Hexaware.Models;

namespace FoodCart_Hexaware.Services
{
    public interface IOrderConfirmationService
    {
        Task SendOrderConfirmationEmailAsync(string toEmail, Orders order);
    }
}
