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
        private readonly GymPower.Services.IEmailService _emailService;

        public CartController(AppDbContext context, GymPower.Services.RecommendationService recommendationService, GymPower.Services.IEmailService emailService)
        {
            _context = context;
            _recommendationService = recommendationService;
            _emailService = emailService;
        }

        private List<CartItem> GetCart()
        {
            var sessionCart = HttpContext.Session.GetString("Cart");
            if (string.IsNullOrEmpty(sessionCart))
                return new List<CartItem>();

            return Newtonsoft.Json.JsonConvert.DeserializeObject<List<CartItem>>(sessionCart) ?? new List<CartItem>();
        }

        // ✅ Shared helper for all AddToCart requests
        private void AddProductToCart(int id, int quantity, int? variantId = null)
        {
            var product = _context.Products.Find(id);
            if (product == null) return;

            // Get variant info if variant was selected
            ProductVariant? variant = null;
            if (variantId.HasValue)
            {
                variant = _context.ProductVariants.Find(variantId.Value);
                if (variant == null || variant.ProductId != id) return; // Invalid variant
            }

            var cartJson = HttpContext.Session.GetString("Cart");
            var cart = string.IsNullOrEmpty(cartJson)
                ? new List<CartItem>()
                : JsonConvert.DeserializeObject<List<CartItem>>(cartJson) ?? new List<CartItem>();

            // Match by both ProductId AND VariantId (products with different variants are separate cart items)
            var existing = cart.FirstOrDefault(x => x.ProductId == id && x.VariantId == variantId);
            if (existing != null)
                existing.Quantity += quantity;
            else
                cart.Add(new CartItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Price = product.Price + (variant?.PriceAdjustment ?? 0), // Add variant price adjustment
                    ImageUrl = product.ImageUrl,
                    Quantity = quantity,
                    VariantId = variant?.Id,
                    VariantType = variant?.VariantType,
                    VariantValue = variant?.VariantValue
                });

            HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));
        }

        // ✅ Handles both Products page buttons (GET) and Details page form (POST)
        [HttpGet, HttpPost]
        public IActionResult AddToCart(int id, int quantity = 1, int? variantId = null)
        {
            var product = _context.Products.Find(id);
            if (product == null)
                return NotFound();

            AddProductToCart(id, quantity, variantId);
            
            // Build message with variant info if present
            var message = $"{product.Name} беше добавен в количката!";
            if (variantId.HasValue)
            {
                var variant = _context.ProductVariants.Find(variantId.Value);
                if (variant != null)
                {
                    message = $"{product.Name} ({variant.VariantType}: {variant.VariantValue}) беше добавен в количката!";
                }
            }
            TempData["SuccessMessage"] = message;

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
        public async Task<IActionResult> PlaceOrder(string FullName, string Address, string Email, string Phone, string PaymentMethod)
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

            // ✅ 3. Validate Stock and Create Order
    foreach (var item in cart)
    {
        var product = _context.Products.FirstOrDefault(p => p.Id == item.ProductId);
        if (product == null) continue;

        if (product.StockQuantity < item.Quantity)
        {
            TempData["ErrorMessage"] = $"Няма достатъчно количество от {product.Name}. Налични: {product.StockQuantity}";
            return RedirectToAction("Index");
        }
    }

    // ✅ 4. Create Order
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

    // ✅ 5. Create OrderItems and Decrement Stock
    order.OrderItems = new List<OrderItem>();
    string orderItemsHtml = "<ul>";
    foreach (var c in cart)
    {
        var product = _context.Products.FirstOrDefault(p => p.Id == c.ProductId);
        if (product != null)
        {
            // Decrement stock
            product.StockQuantity -= c.Quantity;

            order.OrderItems.Add(new OrderItem
            {
                ProductId = c.ProductId,
                Quantity = c.Quantity,
                Price = c.Price,
                VariantId = c.VariantId,
                VariantType = c.VariantType,
                VariantValue = c.VariantValue
            });

            string variantText = string.IsNullOrEmpty(c.VariantValue) ? "" : $" ({c.VariantType}: {c.VariantValue})";
            orderItemsHtml += $"<li style='margin-bottom: 5px;'><b>{c.ProductName}</b>{variantText} - {c.Quantity} бр. x €{(c.Price * 0.51m):0.00}</li>";
        }
    }
    orderItemsHtml += "</ul>";
    
    // Link order to logged-in user if available
    var currentUserId = HttpContext.Session.GetInt32("UserId");
    if (currentUserId.HasValue)
    {
        order.UserId = currentUserId.Value;
    }
    
    _context.Orders.Add(order);
    TempData["LastOrderEmail"] = order.Email;
    await _context.SaveChangesAsync();

            // ✅ 6. Clear the cart
            HttpContext.Session.Remove("Cart");

            // ✅ 7. Send automated email receipt using the new email service
            string subject = $"GymPower - Успешна поръчка #{order.Id}";
            string body = $@"
                <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; color: #333;'>
                    <div style='background-color: #212529; padding: 20px; text-align: center; border-radius: 8px 8px 0 0;'>
                        <h1 style='color: #ffc107; margin: 0;'>GymPower</h1>
                    </div>
                    <div style='padding: 20px; border: 1px solid #ddd; border-top: none; border-radius: 0 0 8px 8px;'>
                        <h2 style='color: #28a745;'>Здравейте, {order.CustomerName}!</h2>
                        <p>Благодарим ви, че избрахте GymPower. Вашата поръчка <b>#{order.Id}</b> беше успешно приета и вече се обработва.</p>
                        <h3 style='border-bottom: 2px solid #ffc107; padding-bottom: 5px; margin-top: 25px;'>Детайли за поръчката</h3>
                        {orderItemsHtml}
                        <h3 style='margin-top: 20px;'>Обща сума: <span style='color: #ffc107;'>€{(order.TotalPrice * 0.51m):0.00}</span></h3>
                        <p style='margin-top: 30px; font-size: 0.9em; color: #666;'>Ако имате въпроси относно поръчката, не се колебайте да се свържете с нашия екип.</p>
                    </div>
                </div>";

            if (!string.IsNullOrEmpty(order.Email))
            {
                await _emailService.SendEmailAsync(order.Email, subject, body);
            }

            // ✅ 8. Success message
            TempData["SuccessMessage"] = "✅ Поръчката е приета успешно!";
            return RedirectToAction("OrderSuccess", new { id = order.Id });
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
        public IActionResult OrderSuccess(int id)
        {
            var order = _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(i => i.Product)
                .FirstOrDefault(o => o.Id == id);
                
            if (order == null)
            {
                return RedirectToAction("Index", "Home");
            }
            
            return View("OrderSuccess", order);
        }
    }
    
}