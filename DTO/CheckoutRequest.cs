using FoodCart_Hexaware.Models;

namespace FoodCart_Hexaware.DTO
{
    public class CheckoutRequest
    {
        
            public int UserId { get; set; } 

            public int RestaurantID { get; set; }
            public string ShippingAddress { get; set; } 
            public string PaymentMethod { get; set; } 


           public string StripePaymentMethodId { get; set; }

    }
}
