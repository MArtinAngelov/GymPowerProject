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

            // Dynamically fetch other items that exist in the database and complement the category
            // (E.g. by querying different available categories dynamically from DB)
            var allCategories = await _context.Products.Select(p => p.Category).Distinct().ToListAsync();
            var targetCategories = allCategories.Where(c => !cartCategories.Contains(c)).OrderBy(x => Guid.NewGuid()).Take(2).ToList();

            var freqList = await _context.Products
                .Where(p => p.StockQuantity > 0 && targetCategories.Contains(p.Category))
                .ToListAsync();

            if (!freqList.Any()) 
            {
                // Absolute fallback to any distinct product
                freqList = await _context.Products.Where(p => p.StockQuantity > 0).ToListAsync();
            }

            return freqList
                .OrderBy(r => Guid.NewGuid())
                .Take(maxItems)
                .ToList();
        }

        private List<string> GetCategoriesForGoal(string goal)
        {
            if (string.IsNullOrEmpty(goal)) return new List<string>();
            
            // Dynamically filter categories that contain the keywords, matching DB data rather than hardcoded rules
            var keywords = goal.ToLower().Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
            
            var allCategories = _context.Products.Select(p => p.Category).Distinct().ToList();
            var matchedCategories = allCategories.Where(c => keywords.Any(k => c.ToLower().Contains(k))).ToList();
            
            if (!matchedCategories.Any()) 
            {
                // Fallback to random categories
                matchedCategories = allCategories.OrderBy(x => Guid.NewGuid()).Take(2).ToList();
            }
            
            return matchedCategories;
        }
    }
}
