using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GymPower.Data;
using GymPower.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GymPower.Controllers
{
    public class UsersController : Controller
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ Dashboard + Search + Filter
        public async Task<IActionResult> Index(string search, string roleFilter)
        {
            var usersQuery = _context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(search))
                usersQuery = usersQuery.Where(u => u.Username.Contains(search) || u.Email.Contains(search));

            if (!string.IsNullOrEmpty(roleFilter) && roleFilter != "Всички")
                usersQuery = usersQuery.Where(u => u.Role == roleFilter);

            var users = await usersQuery.OrderByDescending(u => u.CreatedAt).ToListAsync();

            // Stats
            ViewBag.TotalUsers = await _context.Users.CountAsync();
            ViewBag.Admins = await _context.Users.CountAsync(u => u.Role == "Admin");
            ViewBag.Customers = await _context.Users.CountAsync(u => u.Role != "Admin");
            ViewBag.NewToday = await _context.Users.CountAsync(u => u.CreatedAt.Date == DateTime.Today);

            return View(users);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
                return NotFound();

            var orders = await _context.Orders
                .Where(o => o.CustomerName == user.Username)
                .Include(o => o.OrderItems)
                .ToListAsync();

            ViewBag.Orders = orders;
            return View(user);
        }
        // ✅ Toggle user activation via AJAX
        [HttpPost]
        public IActionResult ToggleActive(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null) return NotFound();

            user.IsActive = !user.IsActive;
            _context.SaveChanges();

            return Json(new { success = true, isActive = user.IsActive });
        }

        // ✅ Create
        public IActionResult Create() => View();

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user)
        {
            if (ModelState.IsValid)
            {
                user.CreatedAt = DateTime.Now;
                _context.Add(user);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "✅ Потребителят е добавен успешно!";
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // ✅ Edit
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();
            return View(user);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, User form)
        {
            if (id != form.Id) return NotFound();
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            if (string.IsNullOrWhiteSpace(form.Password))
                form.Password = user.Password;

            user.Username = form.Username;
            user.Email = form.Email;
            user.Password = form.Password;
            user.Role = form.Role;
            user.FitnessGoal = form.FitnessGoal;
            user.IsActive = form.IsActive;

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "✅ Промените са запазени успешно!";
            return RedirectToAction(nameof(Index));
        }

        // ✅ Delete
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
