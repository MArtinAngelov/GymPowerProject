using GymPower.Data;
using GymPower.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GymPower.Controllers
{
    public class OrdersController : Controller
    {
        private readonly AppDbContext _context;

        public OrdersController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ Show all orders for admin
        public IActionResult Index()
        {
            var orders = _context.Orders
                .Include(o => o.OrderItems)
                .ToList();

            // Debug check — if no data, return empty list
            if (orders == null || !orders.Any())
            {
                ViewBag.Message = "Няма направени поръчки.";
            }

            return View(orders);
        }
        public IActionResult MyOrders()
        {
            // ✅ Try to identify the user
            var username = HttpContext.Session.GetString("Username");
            var email = HttpContext.Session.GetString("Email");

            // Fallback: if session expired but user just ordered, find last used email
            if (string.IsNullOrEmpty(email) && TempData["LastOrderEmail"] != null)
                email = TempData["LastOrderEmail"].ToString();

            var orders = _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(i => i.Product)
                .Where(o => o.CustomerName == username || o.Email == email)
                .OrderByDescending(o => o.OrderDate)
                .ToList();

            if (!orders.Any())
            {
                ViewBag.Message = "Все още нямате направени поръчки.";
            }

            return View(orders);
        }
        public IActionResult MyOrderDetails(int id)
        {
            var username = HttpContext.Session.GetString("Username");
            var email = HttpContext.Session.GetString("Email");

            var order = _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(i => i.Product)
                .FirstOrDefault(o => o.Id == id && (o.CustomerName == username || o.Email == email));

            if (order == null)
                return RedirectToAction("MyOrders");

            return View(order);
        }
        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (order == null)
                return NotFound();

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Order order)
        {
            if (ModelState.IsValid)
            {
                order.OrderDate = DateTime.Now;
                order.Status = "Обработва се";
                order.TotalPrice = order.OrderItems?.Sum(i => i.Price * i.Quantity) ?? 0;

                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
                return NotFound();

            return View(order);
        }

        // POST: Orders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Order order)
        {
            if (id != order.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var order = await _context.Orders
                .FirstOrDefaultAsync(m => m.Id == id);

            if (order == null)
                return NotFound();

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }

        [HttpPost]
        public IActionResult UpdateStatus(int id, string status)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == id);
            if (order != null)
            {
                order.Status = status;
                _context.SaveChanges();
                TempData["SuccessMessage"] = "✅ Статусът на поръчката беше обновен успешно!";
            }

            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult UpdateStatusAjax(int id, string status)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == id);
            if (order != null)
            {
                order.Status = status;
                _context.SaveChanges();
            }
            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult DeleteAjax(int id)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                _context.SaveChanges();
            }
            return Json(new { success = true });
        }
    }

}