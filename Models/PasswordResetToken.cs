using FoodCart_Hexaware.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class PasswordResetToken
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Token { get; set; }

    [Required]
    public DateTime ExpiryDate { get; set; }

    [Required]
    [ForeignKey("Users")]
    public int UserID { get; set; }

    public Users Users { get; set; }
}
