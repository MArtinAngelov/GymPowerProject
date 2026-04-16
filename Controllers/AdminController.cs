using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace GymPower.Controllers
{
    public class AdminController : Controller
    {
        private readonly GymPower.Services.InsightsService _insightsService;
        private readonly GymPower.Data.AppDbContext _context;

        public AdminController(GymPower.Services.InsightsService insightsService, GymPower.Data.AppDbContext context)
        {
            _insightsService = insightsService;
            _context = context;
        }

        // ✅ Main Admin Panel
        public IActionResult Index()
        {
            // Check if user is logged in and is admin
            var role = HttpContext.Session.GetString("Role");

            if (role != "Admin")
            {
                // ❌ Not an admin — redirect home (not login)
                TempData["ErrorMessage"] = "Нямате достъп до Админ панела.";
                return RedirectToAction("Index", "Home");
            }

            // ✅ Already admin — show dashboard
            return View();
        }

        // ✅ AI Insights Dashboard
        public IActionResult Insights()
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != "Admin") return RedirectToAction("Index", "Home");

            var viewModel = _insightsService.GetDashboardData();
            return View(viewModel);
        }
    }
}