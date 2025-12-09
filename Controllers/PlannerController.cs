using GymPower.Data;
using GymPower.Models;
using GymPower.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http; // For Session.GetInt32
using System.Linq; // For .Where

namespace GymPower.Controllers
{
    public class PlannerController : Controller
    {
        private readonly FreeAIService _aiService;
        private readonly AppDbContext _context;

        public PlannerController(FreeAIService aiService, AppDbContext context)
        {
            _aiService = aiService;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Generate()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");

            var user = await _context.Users.FindAsync(userId);
            if (user == null) return RedirectToAction("Login", "Account");

            // Basic Validation: If user hasn't filled profile, redirect to edit
            if (user.Age == 0 || string.IsNullOrEmpty(user.Gender) || user.Gender == "Not Specified")
            {
                TempData["Error"] = "Моля, попълнете профила си (Възраст, Пол, Ниво), за да генерираме план.";
                return RedirectToAction("EditProfile", "Account");
            }

            var plan = await _aiService.GetFitnessPlanAsync(user);

            // Populate Product Objects
            if (plan.RecommendedProductIds != null && plan.RecommendedProductIds.Any())
            {
                plan.RecommendedProducts = await _context.Products
                    .Where(p => plan.RecommendedProductIds.Contains(p.Id))
                    .ToListAsync();
            }

            return View("Result", plan);
        }
    }
}
