using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymPower.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Product Name")]
        public string Name { get; set; }

        [StringLength(500)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        [Range(0.01, 1000)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Display(Name = "Image URL")]
        public string ImageUrl { get; set; }

        [Required]
        public string Category { get; set; } // Protein, Creatine, Vitamins, Accessories

        [Display(Name = "Recommended for Mass Gain")]
        public bool IsRecommendedForMassGain { get; set; }

        [Display(Name = "Recommended for Weight Loss")]
        public bool IsRecommendedForWeightLoss { get; set; }

        [Display(Name = "Recommended for Maintenance")]
        public bool IsRecommendedForMaintenance { get; set; }

        [Display(Name = "Stock Quantity")]
        [Range(0, 1000)]
        public int StockQuantity { get; set; } = 100;
    }
}