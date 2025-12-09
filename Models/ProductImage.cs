using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymPower.Models
{
    public class ProductImage
    {
        [Key]
        public int Id { get; set; }

        public int ProductId { get; set; }

        [Required]
        public string ImageUrl { get; set; } = string.Empty;

        [ForeignKey("ProductId")]
        public Product Product { get; set; } = null!;
    }
}
