using System;
using System.ComponentModel.DataAnnotations;

namespace GymPower.Models
{
    public class UserGoalPreference
    {
        [Key]
        public int Id { get; set; }

        // Nullable for guest users
        public int? UserId { get; set; }

        // For guest tracking via session
        public string? SessionId { get; set; }

        [Required]
        [StringLength(100)]
        public string Goal { get; set; } = string.Empty; // "Изграждане на мускулна маса", "Отслабване", "Енергия", "Регенерация"

        [Required]
        [StringLength(50)]
        public string ExperienceLevel { get; set; } = "Beginner"; // "Beginner", "Intermediate", "Advanced"

        [Required]
        [StringLength(50)]
        public string Budget { get; set; } = "Medium"; // "Low", "Medium", "High"

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [DataType(DataType.DateTime)]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Navigation property
        public User? User { get; set; }
    }
}
