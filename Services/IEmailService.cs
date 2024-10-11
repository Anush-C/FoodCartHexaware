namespace FoodCart_Hexaware.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string message, bool v);
    }
}
