using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Login(string username, string password)
        {
            // ✅ Hardcoded Admin credentials
            if (username == "admin" && password == "1234")
            {
                HttpContext.Session.SetString("Username", "admin");
                HttpContext.Session.SetString("Role", "Admin");

                // Redirect to homepage instead of staying on login page
                return RedirectToAction("Index", "Home");
            }

            // ✅ Normal user login from database
            var user = _context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
            if (user != null)
            {
                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetString("Role", "User");

                return RedirectToAction("Index", "Home");
            }

            // ❌ Invalid credentials
            ViewBag.Error = "Невалидно потребителско име или парола.";
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            if (_context.Users.Any(u => u.Username == user.Username))
            {
                ModelState.AddModelError("Username", "Username already exists");
            }

            if (_context.Users.Any(u => u.Email == user.Email))
            {
                ModelState.AddModelError("Email", "Email already exists");
            }

            if (ModelState.IsValid)
            {
                user.Role = "Customer";
                _context.Users.Add(user);
                _context.SaveChanges();

                HttpContext.Session.SetInt32("UserId", user.Id);
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
        // ✅ Моят профил

public IActionResult EditProfile()
    {
        string username = Convert.ToString(HttpContext.Session.GetString("Username"));
        if (string.IsNullOrEmpty(username))
            return RedirectToAction("Login");

        var user = _context.Users.FirstOrDefault(u => u.Username == username);
        if (user == null)
            return RedirectToAction("Login");

        return View(user);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EditProfile(AppUser form)
    {
        string username = Convert.ToString(HttpContext.Session.GetString("Username"));
        if (string.IsNullOrEmpty(username))
            return RedirectToAction("Login");

        var user = _context.Users.FirstOrDefault(u => u.Username == username);
        if (user == null)
            return RedirectToAction("Login");

        // 🔧 Disable server-side validation for password when left empty
        if (string.IsNullOrWhiteSpace(form.Password))
        {
            ModelState.Remove(nameof(AppUser.Password));
        }

        if (!ModelState.IsValid)
        {
            // Optional: show errors on screen
            return View(form);
        }

        // ✅ Update only allowed fields
        user.Email = form.Email;
        user.FitnessGoal = form.FitnessGoal;

        // ✅ Update password only when provided
        if (!string.IsNullOrWhiteSpace(form.Password))
            user.Password = form.Password;

        _context.Users.Update(user);
        _context.SaveChanges();

        TempData["SuccessMessage"] = "✅ Промените са запазени успешно!";
        return RedirectToAction("Profile");
    }
    public IActionResult Profile()
        {
            string username = Convert.ToString(HttpContext.Session.GetString("Username"));
            if (string.IsNullOrEmpty(username))
                return RedirectToAction("Login");

            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            if (user == null)
                return RedirectToAction("Login");

            return View(user); // → looks for Views/Account/Profile.cshtml
        }

    }
}