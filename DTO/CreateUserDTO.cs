namespace FoodCart_Hexaware.DTO
{
    public class CreateUserDTO
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string AlternativePhoneNumber { get; set; }
        public string Role { get; set; }
        public int? RestaurantID { get; set; } // Nullable
    }

}
