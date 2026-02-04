using System.ComponentModel.DataAnnotations;


namespace GymPower.Models
{
    public class CartItem
    {
        
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; }

        // Variant tracking (nullable for backward compatibility)
        public int? VariantId { get; set; }
        public string? VariantType { get; set; } // e.g., "Taste", "Color"
        public string? VariantValue { get; set; } // e.g., "Chocolate", "Black"

        public decimal TotalPrice => Price * Quantity;
    }
}
