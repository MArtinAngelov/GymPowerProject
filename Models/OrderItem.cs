using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymPower.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        [ForeignKey("Order")]
        public int OrderId { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        // Variant tracking (nullable for backward compatibility)
        public int? VariantId { get; set; }
        public string? VariantType { get; set; } // e.g., "Taste", "Color"
        public string? VariantValue { get; set; } // e.g., "Chocolate", "Black"

        public Order Order { get; set; }
        public Product Product { get; set; }
    }
}