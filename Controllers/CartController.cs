using GymPower.Data;
using GymPower.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly GymPower.Services.RecommendationService _recommendationService;

        public CartController(AppDbContext context, GymPower.Services.RecommendationService recommendationService)
        {
            _context = context;
            _recommendationService = recommendationService;
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
                : JsonConvert.DeserializeObject<List<CartItem>>(cartJson) ?? new List<CartItem>();

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
        public async Task<IActionResult> Index()
        {
            var cartJson = HttpContext.Session.GetString("Cart");
            var cart = string.IsNullOrEmpty(cartJson)
                ? new List<CartItem>()
                : JsonConvert.DeserializeObject<List<CartItem>>(cartJson) ?? new List<CartItem>();
            
            // ✅ Smart Frequent Recommendations
            if (cart.Any())
            {
               // We need product categories. CartItem doesn't store category, so we fetch product details or re-fetch from DB.
               // Optimization: Fetch categories based on ProductIds in cart.
               var productIds = cart.Select(c => c.ProductId).ToList();
               var categories = await _context.Products
                   .Where(p => productIds.Contains(p.Id))
                   .Select(p => p.Category)
                   .Distinct()
                   .ToListAsync();

               var freqProducts = await _recommendationService.GetFrequentlyBoughtTogetherAsync(categories, 3);
               
               // Remove items already in cart
               ViewBag.FreqBought = freqProducts.Where(p => !productIds.Contains(p.Id)).ToList();
            }

            return View(cart);
        }

        // ✅ Remove single item
        public IActionResult RemoveFromCart(int id)
        {
            var cartJson = HttpContext.Session.GetString("Cart");
            if (string.IsNullOrEmpty(cartJson)) return RedirectToAction("Index");

            var cart = JsonConvert.DeserializeObject<List<CartItem>>(cartJson) ?? new List<CartItem>();
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
                : JsonConvert.DeserializeObject<List<CartItem>>(cartJson) ?? new List<CartItem>();

            if (!cart.Any())
                return RedirectToAction("Index");

            ViewBag.Total = cart.Sum(x => x.Price * x.Quantity);
            return View(cart);
        }
        [HttpPost]
        public IActionResult PlaceOrder(string FullName, string Address, string Email, string Phone, string PaymentMethod)
        {
            // ✅ 1. Load cart from session
            var cartJson = HttpContext.Session.GetString("Cart");
            if (string.IsNullOrEmpty(cartJson))
            {
                TempData["ErrorMessage"] = "Количката ти е празна.";
                return RedirectToAction("Index");
            }

            var cart = JsonConvert.DeserializeObject<List<CartItem>>(cartJson) ?? new List<CartItem>();
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

        // ✅ Get Cart Count API
        [HttpGet]
        public IActionResult GetCartCount()
        {
            var cartJson = HttpContext.Session.GetString("Cart");
            var cart = string.IsNullOrEmpty(cartJson)
                ? new List<CartItem>()
                : JsonConvert.DeserializeObject<List<CartItem>>(cartJson) ?? new List<CartItem>();

            return Json(new { count = cart.Sum(x => x.Quantity) });
        }

        // ✅ Add this new method right BELOW PlaceOrder()
        public IActionResult OrderSuccess()
        {
            return View("OrderSuccess");
        }
    }
    
}