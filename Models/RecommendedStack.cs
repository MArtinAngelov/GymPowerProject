using System.ComponentModel.DataAnnotations;

namespace GymPower.Models
{
    public class RecommendedStack
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty; // e.g., "Начинаещ мускулен пакет"

        [Required]
        [StringLength(100)]
        public string Goal { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string ExperienceLevel { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Budget { get; set; } = "All"; // "All", "Low", "Medium", "High"

        [Required]
        public string ProductIds { get; set; } = string.Empty; // Comma-separated: "1,27,15"

        public int DisplayOrder { get; set; } = 0;

        public bool IsActive { get; set; } = true;
    }
}
