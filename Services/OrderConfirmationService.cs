using FoodCart_Hexaware.Models;
using MimeKit;
using MailKit.Net.Smtp;
using System.Linq;
using System.Threading.Tasks;

namespace FoodCart_Hexaware.Services
{
    public class OrderConfirmationService : IOrderConfirmationService
    {
        private readonly IConfiguration _configuration;

        public OrderConfirmationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendOrderConfirmationEmailAsync(string toEmail, Orders order)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("FoodCart", "20euee007@skcet.ac.in"));
            emailMessage.To.Add(new MailboxAddress("", toEmail));
            emailMessage.Subject = "Order Confirmation";

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = $@"
                <h1>Order Confirmation</h1>
                <p>Thank you for your order!</p>
                <p><strong>Order ID:</strong> {order.OrderID}</p>
                <p><strong>Total Amount:</strong> ${order.TotalPrice}</p>
                <h2>Order Details:</h2>
                <ul>
                    {string.Join("", order.OrderItems.Select(item => $"<li>{item.MenuItems?.ItemName ?? "Item Name Unavailable"} - {item.Quantity} x ${item.Price}</li>"))}
                </ul>
                <p>We will notify you once your order is shipped.</p>"
            };

            emailMessage.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                try
                {
                    // Connect to the SMTP server
                    await client.ConnectAsync(
                        _configuration["SmtpSettings:Host"],
                        int.Parse(_configuration["SmtpSettings:Port"]),
                        bool.Parse(_configuration["SmtpSettings:EnableSSL"])
                    );

                    // Authenticate
                    await client.AuthenticateAsync(
                        _configuration["SmtpSettings:Username"],
                        _configuration["SmtpSettings:Password"]
                    );

                    // Send the email
                    await client.SendAsync(emailMessage);
                }
                catch (Exception ex)
                {
                    // Handle the exception (log it, rethrow it, etc.)
                    // For example, logging it:
                    Console.WriteLine($"An error occurred while sending email: {ex.Message}");
                }
                finally
                {
                    // Disconnect
                    await client.DisconnectAsync(true);
                }
            }
        }
    }
}
