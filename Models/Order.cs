using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GymPower.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public string CustomerName { get; set; }

        [Required]
        public string Address { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string PaymentMethod { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;

        public decimal TotalPrice { get; set; }

        public string Status { get; set; } = "Pending";

        // Optional link to user (for admin reference)
        public int? UserId { get; set; }
        public User? User { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}