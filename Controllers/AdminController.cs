using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace GymPower.Controllers
{
    public class AdminController : Controller
    {
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
    }
}