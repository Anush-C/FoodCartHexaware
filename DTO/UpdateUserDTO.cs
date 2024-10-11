using System.ComponentModel.DataAnnotations;

namespace FoodCart_Hexaware.DTO
{
    public class UpdateUserDTO
    {
        [Required]
        public int UserID { get; set; } // Assuming you have a UserID to identify the user to be updated

        [Required(ErrorMessage = "Username is required.")]
        [StringLength(50, ErrorMessage = "Username cannot be longer than 50 characters.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string PhoneNumber { get; set; }

        public string AlternativePhoneNumber { get; set; }

        [Required(ErrorMessage = "Role is required.")]
        public string Role { get; set; }

        public int? RestaurantID { get; set; } // Optional for roles other than Hotel Owner
    }
}
