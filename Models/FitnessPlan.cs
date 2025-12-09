using System.Collections.Generic;

namespace GymPower.Models
{
    public class FitnessPlan
    {
        public string WeeklySplit { get; set; } = string.Empty;
        public string NutritionAdvice { get; set; } = string.Empty;
        public List<int> RecommendedProductIds { get; set; } = new List<int>();
        
        // For Display Logic (populated after AI returns IDs)
        public List<Product> RecommendedProducts { get; set; } = new List<Product>();
    }
}
