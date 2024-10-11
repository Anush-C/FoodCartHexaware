using Stripe;

namespace FoodCart_Hexaware.Services
{
    public interface IStripeGatewayService
    {
        Task<PaymentIntent> CreatePaymentIntent(decimal amount, string currency, string paymentMethodId, string returnUrl);
    }
}
