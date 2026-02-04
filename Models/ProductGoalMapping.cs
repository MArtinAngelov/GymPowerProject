using System.ComponentModel.DataAnnotations;

namespace GymPower.Models
{
    public class ProductGoalMapping
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        [StringLength(100)]
        public string Goal { get; set; } = string.Empty; // "Изграждане на мускулна маса", "Отслабване", "Енергия", "Регенерация"

        [Required]
        [StringLength(50)]
        public string ExperienceLevel { get; set; } = "All"; // "All", "Beginner", "Intermediate", "Advanced"

        [Range(1, 10)]
        public int Priority { get; set; } = 5; // 1-10, for ranking products

        public bool IsBestChoice { get; set; } = false; // Triggers "Най-добър избор" badge

        public bool IsRecommended { get; set; } = false; // Triggers "Подходящо за теб" badge

        // Navigation property
        public Product? Product { get; set; }
    }
}
