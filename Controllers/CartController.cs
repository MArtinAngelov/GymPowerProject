using GymPower.Data;
using GymPower.Models;
using GymPower.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymPower.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDbContext _context;

        public CartController(AppDbContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }
        private List<CartItem> GetCart()
        {
            var sessionCart = HttpContext.Session.GetString("Cart");
            if (string.IsNullOrEmpty(sessionCart))
                return new List<CartItem>();

            return Newtonsoft.Json.JsonConvert.DeserializeObject<List<CartItem>>(sessionCart);
        }

        // ✅ Shared helper for all AddToCart requests
        private void AddProductToCart(int id, int quantity)
        {
            var product = _context.Products.Find(id);
            if (product == null) return;

            var cartJson = HttpContext.Session.GetString("Cart");
            var cart = string.IsNullOrEmpty(cartJson)
                ? new List<CartItem>()
                : JsonConvert.DeserializeObject<List<CartItem>>(cartJson);

            var existing = cart.FirstOrDefault(x => x.ProductId == id);
            if (existing != null)
                existing.Quantity += quantity;
            else
                cart.Add(new CartItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Price = product.Price,
                    ImageUrl = product.ImageUrl,
                    Quantity = quantity
                });

            HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));
        }

        // ✅ Handles both Products page buttons (GET) and Details page form (POST)
        [HttpGet, HttpPost]
        public IActionResult AddToCart(int id, int quantity = 1)
        {
            var product = _context.Products.Find(id);
            if (product == null)
                return NotFound();

            AddProductToCart(id, quantity);
            TempData["SuccessMessage"] = $"{product.Name} беше добавен в количката!";

            // 🔹 Detect AJAX call (from fetch in Details page)
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return Json(new { success = true });

            // 🔹 Detect normal link (from Products page)
            var referer = Request.Headers["Referer"].ToString();
            if (!string.IsNullOrEmpty(referer))
                return Redirect(referer);

            // 🔹 Default fallback
            return RedirectToAction("Index", "Products");
        }

        // ✅ Show cart
        public IActionResult Index()
        {
            var cartJson = HttpContext.Session.GetString("Cart");
            var cart = string.IsNullOrEmpty(cartJson)
                ? new List<CartItem>()
                : JsonConvert.DeserializeObject<List<CartItem>>(cartJson);

            return View(cart);
        }

        // ✅ Remove single item
        public IActionResult RemoveFromCart(int id)
        {
            var cartJson = HttpContext.Session.GetString("Cart");
            if (string.IsNullOrEmpty(cartJson)) return RedirectToAction("Index");

            var cart = JsonConvert.DeserializeObject<List<CartItem>>(cartJson);
            var item = cart.FirstOrDefault(x => x.ProductId == id);
            if (item != null)
                cart.Remove(item);

            HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));
            return RedirectToAction("Index");
        }

        // ✅ Clear cart
        public IActionResult ClearCart()
        {
            HttpContext.Session.Remove("Cart");
            return RedirectToAction("Index");
        }

        // ✅ Checkout
        public IActionResult Checkout()
        {
            var cartJson = HttpContext.Session.GetString("Cart");
            var cart = string.IsNullOrEmpty(cartJson)
                ? new List<CartItem>()
                : JsonConvert.DeserializeObject<List<CartItem>>(cartJson);

            if (!cart.Any())
                return RedirectToAction("Index");

            ViewBag.Total = cart.Sum(x => x.Price * x.Quantity);
            return View(cart);
        }
        [HttpPost]
        public async Task<IActionResult> PlaceOrder(string FullName, string Address, string Email, string Phone, string PaymentMethod)
        {
            // ✅ 1. Load cart from session
            var cartJson = HttpContext.Session.GetString("Cart");
            if (string.IsNullOrEmpty(cartJson))
            {
                TempData["ErrorMessage"] = "Количката ти е празна.";
                return RedirectToAction("Index");
            }

            var cart = JsonConvert.DeserializeObject<List<CartItem>>(cartJson);
            if (cart == null || !cart.Any())
            {
                TempData["ErrorMessage"] = "Количката ти е празна.";
                return RedirectToAction("Index");
            }

            // ✅ 2. Calculate total
            decimal total = cart.Sum(c => c.Price * c.Quantity);

            // ✅ Identify the logged-in user
            var sessionUsername = HttpContext.Session.GetString("Username");
            var sessionEmail = HttpContext.Session.GetString("Email");

            // ✅ Create order
            var order = new Order
            {
                CustomerName = !string.IsNullOrEmpty(sessionUsername) ? sessionUsername : FullName,
                Email = !string.IsNullOrEmpty(sessionEmail) ? sessionEmail : Email,
                Address = Address,
                Phone = Phone,
                PaymentMethod = PaymentMethod,
                TotalPrice = total,
                OrderDate = DateTime.Now,
                Status = "Обработва се"
            };
            // ✅ 4. Create related OrderItems
            order.OrderItems = cart.Select(c => new OrderItem
            {
                ProductId = c.ProductId,
                Quantity = c.Quantity,
                Price = c.Price,
            }).ToList();
            
            _context.Orders.Add(order);
            TempData["LastOrderEmail"] = order.Email;
            _context.SaveChanges();

            string subject = "🎉 Потвърждение на поръчка - GymPower";

            // Build the product table with images
            string productTable = @"
<table style='width:100%; border-collapse:collapse; margin-top:15px; font-family:Arial, sans-serif;'>
<tr style='background-color:#ff6b35; color:white; text-align:left;'>
    <th style='padding:10px;'>Продукт</th>
    <th style='padding:10px;'>Количество</th>
    <th style='padding:10px;'>Цена</th>
    <th style='padding:10px;'>Общо</th>
</tr>";

            foreach (var item in order.OrderItems)
            {
                var product = _context.Products.FirstOrDefault(p => p.Id == item.ProductId);
                if (product != null)
                {
                    productTable += $@"
        <tr style='border-bottom:1px solid #eee; background-color:#fff;'>
            <td style='padding:10px; color:#111;'>{product.Name}</td>
            <td style='padding:10px;'>
                <img src='https://yourdomain.com' alt='{product.Name}' width='80' style='border-radius:8px; border:1px solid #ddd;'/>
            </td>
            <td style='padding:10px; color:#333;'>{item.Quantity}</td>
            <td style='padding:10px; color:#333;'>{item.Price:F2} лв.</td>
            <td style='padding:10px; color:#333;'>{(item.Price * item.Quantity):F2} лв.</td>
        </tr>";
                }
            }
            productTable += "</table>";

            string body = $@"
<div style='font-family:Arial, sans-serif; color:#333; background-color:#f8f9fa; padding:30px; border-radius:10px;'>
    <div style='text-align:center; margin-bottom:25px;'>
        <img src='https://yourdomain.com/images/logo.png' width='120' alt='GymPower Logo' style='margin-bottom:10px;'/>
        <h2 style='color:#ff6b35; margin:0;'>Благодарим ти, {FullName}!</h2>
        <p style='margin-top:5px; color:#555;'>Поръчката ти беше приета успешно на <strong>{order.OrderDate:dd.MM.yyyy HH:mm}</strong>.</p>
    </div>

    <div style='background:white; border-radius:10px; padding:20px; border:1px solid #eee;'>
        <h3 style='color:#ff6b35;'>🛍️ Детайли за поръчката</h3>
        {productTable}

        <p style='margin-top:20px; font-size:16px; color:#111;'>
            <strong>Начин на плащане:</strong> {PaymentMethod}<br>
            <strong>Обща сума:</strong> {total:F2} лв.
        </p>
    </div>

    <hr style='margin:30px 0; border:none; border-top:1px solid #ddd;'>

    <div style='text-align:center;'>
        <p style='color:#555; font-size:15px;'>Ще се свържем с теб на <strong>{Phone}</strong> или на имейл <strong>{Email}</strong> за потвърждение на доставката.</p>
        <p style='color:#777; font-size:13px; margin-top:20px;'>
            💪 Екипът на <strong>GymPower</strong><br>
            <a href='https://localhost:7064/' style='color:#ff6b35; text-decoration:none;'>Посети нашия уебсайт</a> за още оферти и нови продукти.
        </p>
    </div>
</div>
";
            await _emailService.SendEmailAsync("usgympower@gmail.com", $"Нова поръчка от {FullName}", body);
            await _emailService.SendEmailAsync(Email, subject, body);

            // Save order info to TempData for success page
            TempData["OrderId"] = order.Id.ToString();
            TempData["OrderTotal"] = total.ToString("F2");
            TempData["OrderItems"] = JsonConvert.SerializeObject(
           order.OrderItems.Select(i => new {
        ProductId = i.ProductId,
        ProductName = _context.Products.FirstOrDefault(p => p.Id == i.ProductId)?.Name,
        ImageUrl = _context.Products.FirstOrDefault(p => p.Id == i.ProductId)?.ImageUrl,
        Quantity = i.Quantity,
        Price = i.Price
    }).ToList()

);

            // ✅ 6. Clear the cart
            HttpContext.Session.Remove("Cart");

            // ✅ 7. Success message
            TempData["SuccessMessage"] = "✅ Поръчката е приета успешно!";
            return RedirectToAction("OrderSuccess");
        }

        // ✅ Add this new method right BELOW PlaceOrder()
        public IActionResult OrderSuccess()
        {
            return View("OrderSuccess");
        }

        private readonly EmailService _emailService;

      
    }
    
}