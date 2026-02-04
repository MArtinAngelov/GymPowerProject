using GymPower.Data;
using GymPower.Models;
using GymPower.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymPower.Controllers
{
    public class GoalSelectorController : Controller
    {
        private readonly GoalSelectorService _goalService;
        private readonly AppDbContext _context;

        public GoalSelectorController(GoalSelectorService goalService, AppDbContext context)
        {
            _goalService = goalService;
            _context = context;
        }

        // GET: GoalSelector/Index - Show modal/page
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var sessionId = HttpContext.Session.Id;

            var preference = await _goalService.GetPreferenceAsync(userId, sessionId);
            
            return View(preference);
        }

        // POST: GoalSelector/SavePreferences - Save user selections
        [HttpPost]
        public async Task<IActionResult> SavePreferences(string goal, string experienceLevel, string budget)
        {
            if (string.IsNullOrEmpty(goal) || string.IsNullOrEmpty(experienceLevel) || string.IsNullOrEmpty(budget))
            {
                return BadRequest("All fields are required");
            }

            var userId = HttpContext.Session.GetInt32("UserId");
            var sessionId = HttpContext.Session.Id;

            await _goalService.SavePreferenceAsync(userId, sessionId, goal, experienceLevel, budget);

            // Store in session for immediate use (even for logged-in users)
            HttpContext.Session.SetString("Goal", goal);
            HttpContext.Session.SetString("ExperienceLevel", experienceLevel);
            HttpContext.Session.SetString("Budget", budget);

            return Json(new { success = true });
        }

        // GET: GoalSelector/GetRecommendations - Fetch filtered products
        public async Task<IActionResult> GetRecommendations()
        {
            var goal = HttpContext.Session.GetString("Goal");
            var experienceLevel = HttpContext.Session.GetString("ExperienceLevel");
            var budget = HttpContext.Session.GetString("Budget");

            if (string.IsNullOrEmpty(goal) || string.IsNullOrEmpty(experienceLevel))
            {
                return Json(new { success = false, message = "No preferences set" });
            }

            var productsWithScores = await _goalService.GetFilteredProductsAsync(goal, experienceLevel, budget ?? "Medium");

            var result = productsWithScores.Select(p => new
            {
                id = p.Product.Id,
                name = p.Product.Name,
                price = p.Product.Price,
                imageUrl = p.Product.ImageUrl,
                score = p.Score,
                isBestChoice = p.IsBestChoice,
                isRecommended = p.IsRecommended,
                isDimmed = p.Score < 30
            }).ToList();

            return Json(new { success = true, products = result });
        }

        // GET: GoalSelector/GetStack - Fetch recommended stack
        public async Task<IActionResult> GetStack()
        {
            var goal = HttpContext.Session.GetString("Goal");
            var experienceLevel = HttpContext.Session.GetString("ExperienceLevel");
            var budget = HttpContext.Session.GetString("Budget");

            if (string.IsNullOrEmpty(goal) || string.IsNullOrEmpty(experienceLevel))
            {
                return Json(new { success = false, message = "No preferences set" });
            }

            var stackData = await _goalService.GetRecommendedStackAsync(goal, experienceLevel, budget ?? "All");

            if (stackData == null)
            {
                return Json(new { success = false, message = "No stack found" });
            }

            var result = new
            {
                success = true,
                stackName = stackData.Stack.Name,
                products = stackData.Products.Select(p => new
                {
                    id = p.Id,
                    name = p.Name,
                    price = p.Price,
                    imageUrl = p.ImageUrl,
                    description = p.Description
                }).ToList()
            };

            return Json(result);
        }

        // POST: GoalSelector/AddStackToCart - Add entire stack to cart
        [HttpPost]
        public async Task<IActionResult> AddStackToCart()
        {
            var goal = HttpContext.Session.GetString("Goal");
            var experienceLevel = HttpContext.Session.GetString("ExperienceLevel");
            var budget = HttpContext.Session.GetString("Budget");

            if (string.IsNullOrEmpty(goal) || string.IsNullOrEmpty(experienceLevel))
            {
                return Json(new { success = false, message = "No preferences set" });
            }

            var stackData = await _goalService.GetRecommendedStackAsync(goal, experienceLevel, budget ?? "All");

            if (stackData == null || !stackData.Products.Any())
            {
                return Json(new { success = false, message = "No stack found" });
            }

            // Get or initialize cart from session
            var cart = HttpContext.Session.Get<List<CartItem>>("Cart") ?? new List<CartItem>();

            // Add each product to cart
            int addedCount = 0;
            foreach (var product in stackData.Products)
            {
                var existingItem = cart.FirstOrDefault(c => c.ProductId == product.Id && c.VariantId == null);

                if (existingItem != null)
                {
                    existingItem.Quantity += 1;
                }
                else
                {
                    cart.Add(new CartItem
                    {
                        ProductId = product.Id,
                        ProductName = product.Name,
                        Price = product.Price,
                        Quantity = 1,
                        ImageUrl = product.ImageUrl ?? "/images/products/default-product.jpg",
                        VariantId = null,
                        VariantType = null,
                        VariantValue = null
                    });
                }
                addedCount++;
            }

            // Save cart back to session
            HttpContext.Session.Set("Cart", cart);

            return Json(new { success = true, count = addedCount });
        }

        // GET: Clear preference (for testing)
        public IActionResult ClearPreference()
        {
            HttpContext.Session.Remove("Goal");
            HttpContext.Session.Remove("ExperienceLevel");
            HttpContext.Session.Remove("Budget");

            return RedirectToAction("Index", "Products");
        }
    }

    // Extension method for session
    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, System.Text.Json.JsonSerializer.Serialize(value));
        }

        public static T? Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : System.Text.Json.JsonSerializer.Deserialize<T>(value);
        }
    }
}
