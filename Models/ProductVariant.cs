using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymPower.Models
{
    public class ProductVariant
    {
        public int Id { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        [Required]
        [MaxLength(50)]
        public string VariantType { get; set; } // e.g., "Taste", "Color"

        [Required]
        [MaxLength(100)]
        public string VariantValue { get; set; } // e.g., "Chocolate", "Red"

        [Column(TypeName = "decimal(10,2)")]
        public decimal PriceAdjustment { get; set; } = 0; // Optional price modifier

        public int StockQuantity { get; set; } = 0;

        public bool IsAvailable { get; set; } = true;

        // Navigation property
        public Product Product { get; set; } = null!;
    }
}
