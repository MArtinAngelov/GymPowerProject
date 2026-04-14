using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using GymPower.Data;
using GymPower.Models;
using AppUser = GymPower.Models.User;

namespace GymPower.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // ✅ Hardcoded Admin credentials
            if (model.Username == "admin" && model.Password == "1234")
            {
                HttpContext.Session.SetString("Username", "admin");
                HttpContext.Session.SetString("Role", "Admin");

                // Redirect to homepage instead of staying on login page
                return RedirectToAction("Index", "Home");
            }

            // ✅ Normal user login from database
            var user = _context.Users.FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password);
            if (user != null)
            {
                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetString("Role", "User");

                return RedirectToAction("Index", "Home");
            }

            // ❌ Invalid credentials
            ModelState.AddModelError(string.Empty, "Невалидно потребителско име или парола.");
            return View(model);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (_context.Users.Any(u => u.Username == model.Username))
            {
                ModelState.AddModelError("Username", "Потребителското име вече е заето.");
                return View(model);
            }

            if (_context.Users.Any(u => u.Email == model.Email))
            {
                ModelState.AddModelError("Email", "Този имейл вече е регистриран.");
                return View(model);
            }

            var user = new User
            {
                Username = model.Username,
                Email = model.Email,
                Password = model.Password,
                FitnessGoal = model.FitnessGoal,
                Role = "Customer"
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetString("Role", user.Role);
            HttpContext.Session.SetString("FitnessGoal", user.FitnessGoal ?? "");

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
        // ✅ Моят профил

        public IActionResult EditProfile()
        {
            string username = HttpContext.Session.GetString("Username") ?? string.Empty;
            if (string.IsNullOrEmpty(username))
                return RedirectToAction("Login");

            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            if (user == null)
                return RedirectToAction("Login");

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditProfile(AppUser form, string confirmPassword)
        {
            string username = HttpContext.Session.GetString("Username") ?? string.Empty;
            if (string.IsNullOrEmpty(username))
                return RedirectToAction("Login");

            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            if (user == null)
                return RedirectToAction("Login");

            // ✅ Validate new username (if changed)
            if (!string.IsNullOrWhiteSpace(form.Username) && form.Username != user.Username)
            {
                if (_context.Users.Any(u => u.Username == form.Username && u.Id != user.Id))
                {
                    ModelState.AddModelError("Username", "Потребителското име вече е заето.");
                    return View(form);
                }
            }

            // 🔧 Disable server-side validation for password when left empty
            if (string.IsNullOrWhiteSpace(form.Password))
            {
                ModelState.Remove(nameof(AppUser.Password));
            }
            else
            {
                if (form.Password != confirmPassword)
                {
                    ModelState.AddModelError("Password", "Паролите не съвпадат");
                }
            }

            if (!ModelState.IsValid)
                return View(form);

            // ✅ Update username if changed
            if (!string.IsNullOrWhiteSpace(form.Username) && form.Username != user.Username)
            {
                user.Username = form.Username;
                HttpContext.Session.SetString("Username", user.Username); // reflect in navbar instantly
            }

            // ✅ Update other fields
            user.Email = form.Email;
            user.FitnessGoal = form.FitnessGoal;
            user.Age = form.Age;
            user.Gender = form.Gender;
            user.TrainingLevel = form.TrainingLevel;

            // ✅ Update password only when provided
            if (!string.IsNullOrWhiteSpace(form.Password))
                user.Password = form.Password;

            _context.Users.Update(user);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "✅ Промените са запазени успешно!";
            return RedirectToAction("EditProfile");
        }

        public IActionResult Profile()
        {
            string username = HttpContext.Session.GetString("Username") ?? string.Empty;
            if (string.IsNullOrEmpty(username))
                return RedirectToAction("Login");

            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            if (user == null)
                return RedirectToAction("Login");

            return View(user); // → looks for Views/Account/Profile.cshtml
        }

        [HttpGet]
        public IActionResult GoogleLogin()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleResponse")
            };
            return Challenge(properties, Microsoft.AspNetCore.Authentication.Google.GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet]
        public async System.Threading.Tasks.Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Влизането през Google беше неуспешно.");
                return RedirectToAction("Login");
            }

            var claims = result.Principal.Identities.FirstOrDefault()?.Claims;
            var email = claims?.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                ModelState.AddModelError(string.Empty, "Не успяхме да извлечем имейл от вашия Google профил.");
                return RedirectToAction("Login");
            }

            // Check if user exists
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                // Register the user automatically
                user = new AppUser
                {
                    Username = email.Split('@')[0],
                    Email = email,
                    Password = "GoogleLoginUser_" + Guid.NewGuid().ToString().Substring(0, 8),
                    Role = "Customer",
                    FitnessGoal = "Maintenance",
                    Age = 0
                };
                
                // Ensure unique username
                if (_context.Users.Any(u => u.Username == user.Username))
                {
                    user.Username = user.Username + "_" + new Random().Next(1000, 9999);
                }

                _context.Users.Add(user);
                _context.SaveChanges();
            }

            // Set session variables just like regular login
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetString("Role", user.Role ?? "Customer");
            HttpContext.Session.SetString("FitnessGoal", user.FitnessGoal ?? "");

            // Clear external cookie
            await HttpContext.SignOutAsync(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }
    }
}