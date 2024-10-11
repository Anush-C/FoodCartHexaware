namespace FoodCart_Hexaware.DTO
{
    public class AddToCartDTO
    {
        public int UserID { get; set; }
        public int ItemID { get; set; }
        public int Quantity { get; set; }

        public int RestaurantID { get; set; }
        
    }
}
