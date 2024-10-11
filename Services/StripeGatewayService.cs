using FoodCart_Hexaware.Models;
using FoodCart_Hexaware.Services;
using Microsoft.Extensions.Options;
using Stripe;

public class StripeGatewayService : IStripeGatewayService
{
    public async Task<PaymentIntent> CreatePaymentIntent(decimal amount, string currency, string paymentMethodId, string returnUrl)
    {
        var paymentIntentService = new PaymentIntentService();
        var paymentIntent = await paymentIntentService.CreateAsync(new PaymentIntentCreateOptions
        {
            Amount = (long)(amount * 100), // Convert to smallest currency unit
            Currency = currency,
            PaymentMethod = paymentMethodId,
            ConfirmationMethod = "manual",
            Confirm = true,
            ReturnUrl = returnUrl,
        });

        return paymentIntent;
    }
}  