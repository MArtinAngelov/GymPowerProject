using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymPower.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } // replaces 'Name'

        [Required]
        public string Description { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        [Required]
        public decimal Price { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        public string Category { get; set; }

        public int StockQuantity { get; set; } = 0;
        public bool IsRecommendedForMassGain { get; set; } = false;
        public bool IsRecommendedForWeightLoss { get; set; } = false;
        public bool IsRecommendedForMaintenance { get; set; } = false;
    }
}