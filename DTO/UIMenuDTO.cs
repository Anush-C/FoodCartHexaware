namespace FoodCart_Hexaware.DTO
{
    public class UIMenuDTO
    {
        
            public string MenuItemName { get; set; }
            public decimal Rating { get; set; }
            
            public List<UIRestaurantDTO> Items { get; set; }
        

    }
}
