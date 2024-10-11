using FoodCart_Hexaware.DTOs;

namespace FoodCart_Hexaware.DTO
{
    public class OrderConfirmationDTO
    {
        public int OrderID { get; set; }
        public string RestaurantName { get; set; }
        public string DeliveryAgentName { get; set; }
        public string DeliveryAgentPhone { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime? DeliveryTime { get; set; }
        public List<OrderItemsDTO> Items { get; set; }
    }
}
