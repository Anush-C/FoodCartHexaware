using System.ComponentModel.DataAnnotations;

namespace FoodCart_Hexaware.DTO
{
    public class ForgotPasswordDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
