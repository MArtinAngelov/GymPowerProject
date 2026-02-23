using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace GymPower.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Column(TypeName = "decimal(10,2)")]
        [Required]
        public decimal Price { get; set; }

        [Required]
        public string ImageUrl { get; set; } = string.Empty;

        [Required]
        public string Category { get; set; } = string.Empty;

        public int StockQuantity { get; set; } = 0;
        public bool IsRecommendedForMassGain { get; set; } = false;
        public bool IsRecommendedForWeightLoss { get; set; } = false;
        public bool IsRecommendedForMaintenance { get; set; } = false;

        public string? LongDescription { get; set; }
        public List<ProductImage> Images { get; set; } = new List<ProductImage>();
        public List<ProductVariant> Variants { get; set; } = new List<ProductVariant>();
    }
}