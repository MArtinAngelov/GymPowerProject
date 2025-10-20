using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GymPower.Data;
using GymPower.Models;

namespace GymPower.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user != null)
            {
                HttpContext.Session.SetString("UserId", user.Id.ToString());
                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetString("Role", user.Role);
                HttpContext.Session.SetString("FitnessGoal", user.FitnessGoal);

                if (user.Role == "Admin")
                    return RedirectToAction("Index", "Admin");

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Invalid username or password";
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                // Check if username already exists
                if (_context.Users.Any(u => u.Username == user.Username))
                {
                    ModelState.AddModelError("Username", "Username already exists");
                    return View(user);
                }

                // Check if email already exists
                if (_context.Users.Any(u => u.Email == user.Email))
                {
                    ModelState.AddModelError("Email", "Email already exists");
                    return View(user);
                }

                user.Role = "Customer";
                _context.Users.Add(user);
                _context.SaveChanges();

                // Auto login after registration
                HttpContext.Session.SetString("UserId", user.Id.ToString());
                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetString("Role", user.Role);
                HttpContext.Session.SetString("FitnessGoal", user.FitnessGoal);

                return RedirectToAction("Index", "Home");
            }

            return View(user);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
