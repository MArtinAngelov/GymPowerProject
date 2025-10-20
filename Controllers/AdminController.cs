using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GymPower.Data;
using GymPower.Models;

namespace GymPower.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        // Check if user is admin
        private bool IsAdmin()
        {
            var role = HttpContext.Session.GetString("Role");
            return role == "Admin";
        }

        public IActionResult Index()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            var stats = new
            {
                TotalProducts = _context.Products.Count(),
                TotalOrders = _context.Orders.Count(),
                TotalUsers = _context.Users.Count(),
                RecentOrders = _context.Orders
                    .Include(o => o.User)
                    .OrderByDescending(o => o.OrderDate)
                    .Take(5)
                    .ToList()
            };

            return View(stats);
        }

        // Product Management
        public IActionResult Products()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            var products = _context.Products.ToList();
            return View(products);
        }

        [HttpGet]
        public IActionResult CreateProduct()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");
            return View();
        }

        [HttpPost]
        public IActionResult CreateProduct(Product product)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {
                _context.Products.Add(product);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Product created successfully!";
                return RedirectToAction("Products");
            }

            return View(product);
        }

        [HttpGet]
        public IActionResult EditProduct(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost]
        public IActionResult EditProduct(Product product)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {
                _context.Products.Update(product);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Product updated successfully!";
                return RedirectToAction("Products");
            }

            return View(product);
        }

        [HttpPost]
        public IActionResult DeleteProduct(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            var product = _context.Products.Find(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Product deleted successfully!";
            }

            return RedirectToAction("Products");
        }

        // Order Management
        public IActionResult Orders()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            var orders = _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .OrderByDescending(o => o.OrderDate)
                .ToList();

            return View(orders);
        }

        [HttpPost]
        public IActionResult UpdateOrderStatus(int orderId, string status)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            var order = _context.Orders.Find(orderId);
            if (order != null)
            {
                order.Status = status;
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Order status updated successfully!";
            }

            return RedirectToAction("Orders");
        }

        // User Management
        public IActionResult Users()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            var users = _context.Users.Where(u => u.Role != "Admin").ToList();
            return View(users);
        }
    }
}