using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FoodCart_Hexaware.Models
{
    public class MenuItems
    {
        [Required]
        [Key]
        public int ItemID { get; set; }

        [Required]
        [StringLength(100)]
        public string ItemName { get; set; }

        [Required]
        public string ItemDescription { get; set; }

        [Required]
        [Range(0.01, 10000.00, ErrorMessage = "Price must be a positive value")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal ItemPrice { get; set; }

        [Required]
        public string Ingredients { get; set; }

        [Required]
        public string CuisineType { get; set; }

        [Required]
        public string TasteInfo { get; set; }
        [Required]
        [Range(0.1, 10000.00, ErrorMessage = "Rating must be a positive value")]
        [Column(TypeName = "decimal(18, 1)")]
        public decimal Rating { get; set; }

        [Required]
        [StringLength(50)]
        public string AvailabilityStatus { get; set; }

        [Required]
        public string DietaryInfo { get; set; }

        public string ImageURL { get; set; }

        [Required]
        [ForeignKey("MenuCategory")]
        public int CategoryID { get; set; }
        [JsonIgnore]


        public MenuCategory? MenuCategory { get; set; }

        [JsonIgnore] // Exclude from JSON serialization to avoid circular references
        public ICollection<OrderItems> OrderItems { get; set; } = new List<OrderItems>();

        [JsonIgnore] // Exclude from JSON serialization to avoid circular references
        public ICollection<Cart> Carts { get; set; } = new List<Cart>();

        [JsonIgnore] // Exclude from JSON serialization to avoid circular references
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();

        [JsonIgnore] // Exclude from JSON serialization to avoid circular references
        public ICollection<Restaurant> Restaurants { get; set; } = new List<Restaurant>();
    }
}
