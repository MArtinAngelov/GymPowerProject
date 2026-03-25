using GymPower.Data;
using GymPower.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymPower.Services
{
    public class InsightsService
    {
        private readonly AppDbContext _context;

        public InsightsService(AppDbContext context)
        {
            _context = context;
        }

        // --- 📊 Main Dashboard Data ---
        public DashboardViewModel GetDashboardData()
        {
            var data = new DashboardViewModel
            {
                TotalRevenue = GetTotalRevenue(),
                TotalOrders = GetTotalOrders(),
                ActiveUsersCount = GetActiveUsersCount(),
                AverageOrderValue = GetAverageOrderValue(),
                TopProducts = GetTopProducts(5),
                CategoryStats = GetCategoryStats(),
                Insights = GetTextInsights()
            };

            return data;
        }

        // --- 🧮 Calculations ---

        private decimal GetTotalRevenue()
        {
            // SQLite specific fix: Cannot Sum(decimal) on server side.
            // Fetching values and summing in memory.
            return _context.Orders
                .Select(o => o.TotalPrice)
                .AsEnumerable()
                .Sum();
        }

        private int GetTotalOrders()
        {
            return _context.Orders.Count();
        }

        private int GetActiveUsersCount()
        {
            // Users who have placed at least one order
            return _context.Users.Count(u => _context.Orders.Any(o => o.UserId == u.Id));
        }

        private decimal GetAverageOrderValue()
        {
            var count = _context.Orders.Count();
            if (count == 0) return 0;

            // SQLite specific fix: Sum on client side
            var totalRevenue = _context.Orders
                .Select(o => o.TotalPrice)
                .AsEnumerable()
                .Sum();

            return totalRevenue / count;
        }

        private List<BestProductDto> GetTopProducts(int count)
        {
            // Join OrderItems -> Products grouping
            // Note: In EF Core, GroupBy is sometimes tricky. We'll do simple logic.
            // If dataset is huge, this should be optimized. For now (MVP), it works.
            
            var query = _context.OrderItems
                .Include(oi => oi.Product)
                .AsEnumerable() // Client-side grouping for simplicity in SQLite 
                .GroupBy(oi => oi.ProductId)
                .Select(g => new BestProductDto
                {
                    ProductId = g.Key,
                    Name = g.First().Product?.Name ?? "Unknown",
                    ImageUrl = g.First().Product?.ImageUrl ?? "/images/default.png",
                    QuantitySold = g.Sum(oi => oi.Quantity),
                    RevenueCode = g.Sum(oi => oi.Price * oi.Quantity)
                })
                .OrderByDescending(p => p.QuantitySold)
                .Take(count)
                .ToList();

            return query;
        }

        private List<CategoryStatsDto> GetCategoryStats()
        {
            // Top categories by revenue
            var query = _context.OrderItems
                .Include(oi => oi.Product)
                .AsEnumerable()
                .GroupBy(oi => oi.Product?.Category ?? "Other")
                .Select(g => new CategoryStatsDto
                {
                    Category = g.Key,
                    Revenue = g.Sum(oi => oi.Price * oi.Quantity)
                })
                .OrderByDescending(c => c.Revenue)
                .ToList();

            return query;
        }

        // --- 🤖 AI Rule-Based Insights ---
        private List<string> GetTextInsights()
        {
            var insights = new List<string>();

            // Data
            var revenue = GetTotalRevenue();
            var orders = GetTotalOrders();
            var aov = GetAverageOrderValue();
            var topCats = GetCategoryStats();
            var topCat = topCats.FirstOrDefault();

            // Rule 1: Category Dominance
            if (topCat != null)
            {
                if (topCat.Category.Contains("Мускул", StringComparison.OrdinalIgnoreCase))
                {
                    insights.Add("💪 **Фокус върху мускулната маса**: Клиентите купуват основно продукти за обем. Препоръчвам да заредиш повече гейнъри и креатин.");
                }
                else if (topCat.Category.Contains("Отслабване", StringComparison.OrdinalIgnoreCase))
                {
                    insights.Add("🔥 **Сезонът на чистенето**: Продуктите за отслабване доминира продажбите. Пусни промоция на Л-Карнитин или Фет Бърнъри.");
                }
                else if (topCat.Category.Contains("Витамин", StringComparison.OrdinalIgnoreCase))
                {
                    insights.Add("💊 **Здравето е приоритет**: Витамините се търсят много. Подготви стак 'Имунитет' за началната страница.");
                }
                else
                {
                     insights.Add($"📈 **Лидер в продажбите**: Категория **{topCat.Category}** носи най-много приходи ({topCat.Revenue:F2} лв).");
                }
            }

            // Rule 2: AOV Analysis
            if (aov < 50 && orders > 5)
            {
                insights.Add("📦 **Ниска стойност на количката**: Средната поръчка е под 50 лв. Опитай да създадеш пакети (Bundles) или промоция 'Безплатна доставка над 60 лв', за да вдигнеш оборота.");
            }
            else if (aov > 100)
            {
                 insights.Add("💎 **Premium Клиенти**: Поздравления! Клиентите купуват скъпи продукти. Поддържай високо качество и добави VIP програма.");
            }

            // Rule 3: Growth / Activity
            if (orders == 0)
            {
                insights.Add("⚠️ **Няма скорошни поръчки**: Време е за маркетинг кампания! Изпрати имейл до потребителите.");
            }
            if (orders > 0 && revenue / orders > 80) 
            {
                 insights.Add("🚀 **Висока ефективност**: Правиш малко на брой, но големи поръчки. Това е добре за логистиката.");
            }

            // Fallback
            if (!insights.Any())
            {
                insights.Add("📊 Няма достатъчно данни за генериране на инсайти. Изчакай първите няколко поръчки!");
            }

            return insights;
        }
    }

    // --- DTO Classes ---
    public class DashboardViewModel
    {
        public decimal TotalRevenue { get; set; }
        public int TotalOrders { get; set; }
        public int ActiveUsersCount { get; set; }
        public decimal AverageOrderValue { get; set; }
        public List<BestProductDto> TopProducts { get; set; } = new();
        public List<CategoryStatsDto> CategoryStats { get; set; } = new();
        public List<string> Insights { get; set; } = new();
    }

    public class BestProductDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public int QuantitySold { get; set; }
        public decimal RevenueCode { get; set; }
    }

    public class CategoryStatsDto
    {
        public string Category { get; set; } = string.Empty;
        public decimal Revenue { get; set; }
    }
}
