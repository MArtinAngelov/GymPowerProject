using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymPower.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "User")]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [Display(Name = "Order Date")]
        [DataType(DataType.DateTime)]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Display(Name = "Total Price")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }

        public string Status { get; set; } = "Pending";

        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}