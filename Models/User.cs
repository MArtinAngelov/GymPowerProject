using System.ComponentModel.DataAnnotations;

namespace GymPower.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string Role { get; set; } = "Customer";

        [Display(Name = "Fitness Goal")]
        public string FitnessGoal { get; set; } // MassGain, WeightLoss, Maintenance

        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
