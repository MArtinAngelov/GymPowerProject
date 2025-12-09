using GymPower.Data;
using GymPower.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymPower.Services
{
    public class RecommendationService
    {
        private readonly AppDbContext _context;

        public RecommendationService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetRecommendedProductsForUserAsync(int? userId, int maxItems = 4)
        {
            // 1. Base Query: Available products only
            var query = _context.Products.Where(p => p.StockQuantity > 0);

            // 2. Variable to hold potential products
            List<Product> candidates = new List<Product>();

            if (userId.HasValue)
            {
                var user = await _context.Users
                    .Include(u => u.Orders)
                    .ThenInclude(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                    .FirstOrDefaultAsync(u => u.Id == userId.Value);

                if (user != null)
                {
                    // A. Feature Goal Matching
                    var goalCategories = GetCategoriesForGoal(user.FitnessGoal);
                    var goalProductsList = await query
                        .Where(p => goalCategories.Contains(p.Category))
                        .ToListAsync();
                    
                    var goalProducts = goalProductsList
                        .OrderBy(r => Guid.NewGuid()) // Client-side shuffle
                        .Take(maxItems * 2)
                        .ToList();

                    candidates.AddRange(goalProducts);

                    // B. Purchase History Matching (Complementary Categories)
                    if (user.Orders != null && user.Orders.Any())
                    {
                        var purchasedProductIds = user.Orders
                            .SelectMany(o => o.OrderItems)
                            .Select(oi => oi.ProductId)
                            .Distinct()
                            .ToList();

                        var purchasedCategories = user.Orders
                            .SelectMany(o => o.OrderItems)
                            .Select(oi => oi.Product.Category)
                            .Distinct()
                            .ToList();

                        var historyProductsList = await query
                            .Where(p => purchasedCategories.Contains(p.Category) && !purchasedProductIds.Contains(p.Id))
                            .ToListAsync();
                             
                        var historyProducts = historyProductsList
                            .OrderBy(r => Guid.NewGuid())
                            .Take(maxItems)
                            .ToList();

                        candidates.AddRange(historyProducts);
                    }
                }
            }

            // 3. Fallback / Guest: Top rated or Random
            if (!candidates.Any())
            {
                var randomProductsList = await query.ToListAsync();
                var randomProducts = randomProductsList
                    .OrderBy(r => Guid.NewGuid()) 
                    .Take(maxItems)
                    .ToList();
                candidates.AddRange(randomProducts);
            }

            // 4. Final distinct selection
            return candidates
                .DistinctBy(p => p.Id)
                .Take(maxItems)
                .ToList();
        }

        public async Task<List<Product>> GetFrequentlyBoughtTogetherAsync(List<string> cartCategories, int maxItems = 3)
        {
            if (cartCategories == null || !cartCategories.Any())
                return new List<Product>();

            var targetCategories = new List<string>();

            if (cartCategories.Contains("Изграждане на мускулна маса")) 
                targetCategories.AddRange(new[] { "Регенерация", "Аксесоари" });
            
            if (cartCategories.Contains("Отслабване")) 
                targetCategories.AddRange(new[] { "Витамини и Минерали", "Здравословна закуска" });

            if (!targetCategories.Any()) 
                targetCategories.Add("Аксесоари"); // Always safe bet

            var freqList = await _context.Products
                .Where(p => p.StockQuantity > 0 && targetCategories.Contains(p.Category))
                .ToListAsync();

            return freqList
                .OrderBy(r => Guid.NewGuid())
                .Take(maxItems)
                .ToList();
        }

        private List<string> GetCategoriesForGoal(string goal)
        {
            // Normalize
            if (string.IsNullOrEmpty(goal)) return new List<string>();
            
            // Map User.FitnessGoal (which might be "Maintenance", "Muscle", "WeightLoss" - wait, let's check User model values)
            // Model says default "Maintenance". Assuming strings match frontend or we map loosely.
            // Based on seed data, categories are specific strings.

            // Heuristic matching based on likely keywords
            var g = goal.ToLower();
            
            if (g.Contains("мускул") || g.Contains("muscle")) 
                return new List<string> { "Изграждане на мускулна маса", "Висока производителност", "Аминокиселини" };
            
            if (g.Contains("отслаб") || g.Contains("weight") || g.Contains("fat")) 
                return new List<string> { "Отслабване", "Здравословна закуска", "Кардио" };
            
            // Default / Maintenance
            return new List<string> { "Регенерация", "Имунитет", "Витамини и Минерали" };
        }
    }
}
