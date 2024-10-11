namespace FoodCart_Hexaware.DTO
{
    public class RazorPayVerificationRequest
    {
        public string PaymentId { get; set; }
        public int OrderId { get; set; }
        public string Signature { get; set; }
    }
}
