namespace FoodCart_Hexaware.DTO
{
    public class CartDTO
    {
        public int CartID { get; set; } // Assuming you have a Cart ID
        public int Quantity { get; set; }
        public string DeliveryAddress { get; set; }
        public decimal TotalCost { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int ItemID { get; set; }
        public int UserID { get; set; }
        public string ItemName { get; set; } // Add ItemName property

        public string RestaurantName { get; set; }
    }
}
