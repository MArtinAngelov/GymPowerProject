using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GymPower.Data;
using GymPower.Models;
using System.Text.Json;

namespace GymPower.Controllers
{
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;
        private const string CartSessionKey = "Cart";

        public OrderController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Checkout()
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr))
            {
                return RedirectToAction("Login", "Account");
            }

            var cartJson = HttpContext.Session.GetString(CartSessionKey);
            if (string.IsNullOrEmpty(cartJson))
            {
                TempData["ErrorMessage"] = "Your cart is empty";
                return RedirectToAction("Index", "Cart");
            }

            var cart = JsonSerializer.Deserialize<List<CartItem>>(cartJson);
            if (!cart.Any())
            {
                TempData["ErrorMessage"] = "Your cart is empty";
                return RedirectToAction("Index", "Cart");
            }

            var order = new Order
            {
                UserId = int.Parse(userIdStr),
                OrderDate = DateTime.Now,
                TotalPrice = cart.Sum(item => item.TotalPrice),
                Status = "Confirmed"
            };

            _context.Orders.Add(order);
            _context.SaveChanges();

            foreach (var item in cart)
            {
                var orderItem = new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price
                };
                _context.OrderItems.Add(orderItem);
            }

            _context.SaveChanges();

            // Clear cart
            HttpContext.Session.Remove(CartSessionKey);

            TempData["SuccessMessage"] = "Order placed successfully!";
            return RedirectToAction("Details", new { id = order.Id });
        }

        public IActionResult MyOrders()
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr))
            {
                return RedirectToAction("Login", "Account");
            }

            var userId = int.Parse(userIdStr);
            var orders = _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToList();

            return View(orders);
        }

        public IActionResult Details(int id)
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr))
            {
                return RedirectToAction("Login", "Account");
            }

            var order = _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Include(o => o.User)
                .FirstOrDefault(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            // Check if the order belongs to the current user or if user is admin
            var userId = int.Parse(userIdStr);
            var userRole = HttpContext.Session.GetString("Role");
            if (order.UserId != userId && userRole != "Admin")
            {
                return Unauthorized();
            }

            return View(order);
        }
    }
}