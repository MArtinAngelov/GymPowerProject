using GymPower.Data;
using GymPower.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymPower.Services
{
    public class GoalSelectorService
    {
        private readonly AppDbContext _context;

        public GoalSelectorService(AppDbContext context)
        {
            _context = context;
        }

        // Save user preference (session or DB)
        public async Task<UserGoalPreference> SavePreferenceAsync(int? userId, string? sessionId, string goal, string experienceLevel, string budget)
        {
            // Try to find existing preference
            UserGoalPreference? preference = null;
            
            if (userId.HasValue)
            {
                preference = await _context.UserGoalPreferences
                    .FirstOrDefaultAsync(p => p.UserId == userId.Value);
            }
            else if (!string.IsNullOrEmpty(sessionId))
            {
                preference = await _context.UserGoalPreferences
                    .FirstOrDefaultAsync(p => p.SessionId == sessionId);
            }

            if (preference != null)
            {
                // Update existing
                preference.Goal = goal;
                preference.ExperienceLevel = experienceLevel;
                preference.Budget = budget;
                preference.UpdatedAt = DateTime.Now;
            }
            else
            {
                // Create new
                preference = new UserGoalPreference
                {
                    UserId = userId,
                    SessionId = sessionId,
                    Goal = goal,
                    ExperienceLevel = experienceLevel,
                    Budget = budget,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                _context.UserGoalPreferences.Add(preference);
            }

            await _context.SaveChangesAsync();
            return preference;
        }

        // Get user preference
        public async Task<UserGoalPreference?> GetPreferenceAsync(int? userId, string? sessionId)
        {
            if (userId.HasValue)
            {
                return await _context.UserGoalPreferences
                    .FirstOrDefaultAsync(p => p.UserId == userId.Value);
            }
            else if (!string.IsNullOrEmpty(sessionId))
            {
                return await _context.UserGoalPreferences
                    .FirstOrDefaultAsync(p => p.SessionId == sessionId);
            }
            return null;
        }

        // Get filtered products with ranking
        public async Task<List<ProductWithScore>> GetFilteredProductsAsync(string goal, string experienceLevel, string budget)
        {
            // Get all products with their mappings
            var products = await _context.Products
                .Include(p => p.Images)
                .Include(p => p.Variants)
                .Where(p => p.StockQuantity > 0)
                .ToListAsync();

            var mappings = await _context.ProductGoalMappings
                .Where(m => m.Goal == goal && (m.ExperienceLevel == experienceLevel || m.ExperienceLevel == "All"))
                .ToListAsync();

            var productScores = new List<ProductWithScore>();

            foreach (var product in products)
            {
                var mapping = mappings.FirstOrDefault(m => m.ProductId == product.Id);
                int score = CalculatePriority(product, mapping, budget);

                productScores.Add(new ProductWithScore
                {
                    Product = product,
                    Score = score,
                    IsBestChoice = mapping?.IsBestChoice ?? false,
                    IsRecommended = mapping?.IsRecommended ?? false
                });
            }

            // Sort by score descending
            return productScores.OrderByDescending(p => p.Score).ToList();
        }

        // Calculate priority score
        private int CalculatePriority(Product product, ProductGoalMapping? mapping, string budget)
        {
            if (mapping == null) return 0;

            int score = 100; // Base score for matching

            // Add mapping priority (1-10 * 10 = 10-100 points)
            score += mapping.Priority * 10;

            // Best choice bonus
            if (mapping.IsBestChoice)
                score += 50;

            // Recommended bonus
            if (mapping.IsRecommended)
                score += 30;

            // Budget filtering/scoring
            decimal priceBGN = product.Price;
            
            switch (budget)
            {
                case "Low":
                    if (priceBGN > 35) score -= 50; // Penalize expensive products
                    else score += 20; // Bonus for affordable
                    break;
                case "Medium":
                    if (priceBGN < 35) score += 10;
                    else if (priceBGN > 60) score -= 30;
                    else score += 20; // Sweet spot
                    break;
                case "High":
                    if (priceBGN > 60) score += 20; // Premium products
                    break;
            }

            return Math.Max(0, score); // Never negative
        }

        // Get recommended stack
        public async Task<RecommendedStackWithProducts?> GetRecommendedStackAsync(string goal, string experienceLevel, string budget)
        {
            // Find matching stack (prioritize exact match, then "All" budget)
            var stack = await _context.RecommendedStacks
                .Where(s => s.Goal == goal && 
                           s.ExperienceLevel == experienceLevel && 
                           s.IsActive &&
                           (s.Budget == budget || s.Budget == "All"))
                .OrderBy(s => s.Budget == budget ? 0 : 1) // Prefer exact budget match
                .FirstOrDefaultAsync();

            if (stack == null) return null;

            // Parse product IDs
            var productIds = stack.ProductIds
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(id => int.TryParse(id.Trim(), out int x) ? x : -1)
                .Where(id => id > 0)
                .ToList();

            // Fetch products
            var products = await _context.Products
                .Include(p => p.Images)
                .Include(p => p.Variants)
                .Where(p => productIds.Contains(p.Id) && p.StockQuantity > 0)
                .ToListAsync();

            // Maintain order from ProductIds
            var orderedProducts = productIds
                .Select(id => products.FirstOrDefault(p => p.Id == id))
                .Where(p => p != null)
                .ToList();

            return new RecommendedStackWithProducts
            {
                Stack = stack,
                Products = orderedProducts!
            };
        }
    }

    // Helper classes
    public class ProductWithScore
    {
        public Product Product { get; set; } = null!;
        public int Score { get; set; }
        public bool IsBestChoice { get; set; }
        public bool IsRecommended { get; set; }
    }

    public class RecommendedStackWithProducts
    {
        public RecommendedStack Stack { get; set; } = null!;
        public List<Product> Products { get; set; } = new List<Product>();
    }
}
