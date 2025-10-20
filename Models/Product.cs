using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymPower.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Product name is required")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters")]
        [Display(Name = "Product Name")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, 1000, ErrorMessage = "Price must be between 0.01 and 1000")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Display(Name = "Image URL")]
        [Url(ErrorMessage = "Please enter a valid URL")]
        public string ImageUrl { get; set; } = "/images/products/default-product.jpg";

        [Required(ErrorMessage = "Category is required")]
        [Display(Name = "Category")]
        public string Category { get; set; } = "Protein";

        [Display(Name = "Recommended for Mass Gain")]
        public bool IsRecommendedForMassGain { get; set; }

        [Display(Name = "Recommended for Weight Loss")]
        public bool IsRecommendedForWeightLoss { get; set; }

        [Display(Name = "Recommended for Maintenance")]
        public bool IsRecommendedForMaintenance { get; set; }

        [Display(Name = "Stock Quantity")]
        [Range(0, 1000, ErrorMessage = "Stock quantity must be between 0 and 1000")]
        public int StockQuantity { get; set; } = 100;
    }
}