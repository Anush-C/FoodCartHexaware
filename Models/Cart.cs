using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FoodCart_Hexaware.Models
{
    public class Cart
    {
        [Required]
        [Key]
        public int CartID { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        
        public string? DeliveryAddress { get; set; }

        [Required]
        [Range(0.01, 10000.00, ErrorMessage = "Total Cost must be a positive value.")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalCost { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; }


        [ForeignKey("MenuItems")]
        public int ItemID { get; set; }


        [ForeignKey("Users")]
        public int UserID { get; set; }

       
        [JsonIgnore]
        public MenuItems? MenuItems { get; set; }

        [JsonIgnore]
        public Users? Users { get; set; }

        
    }
}
