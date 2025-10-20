using Microsoft.AspNetCore.Mvc;
using GymPower.Models;
using System.Text.Json;

namespace GymPower.Controllers
{
    public class CartController : Controller
    {
        private const string CartSessionKey = "Cart";

        private List<CartItem> GetCart()
        {
            var cartJson = HttpContext.Session.GetString(CartSessionKey);
            if (string.IsNullOrEmpty(cartJson))
                return new List<CartItem>();

            try
            {
                return JsonSerializer.Deserialize<List<CartItem>>(cartJson) ?? new List<CartItem>();
            }
            catch
            {
                return new List<CartItem>();
            }
        }

        private void SaveCart(List<CartItem> cart)
        {
            var cartJson = JsonSerializer.Serialize(cart);
            HttpContext.Session.SetString(CartSessionKey, cartJson);
        }

        public IActionResult Index()
        {
            var cart = GetCart();
            ViewBag.TotalPrice = cart.Sum(item => item.TotalPrice);
            return View(cart);
        }

        [HttpPost]
        public IActionResult AddToCart(int productId, string productName, decimal price, string imageUrl, int quantity = 1)
        {
            try
            {
                var cart = GetCart();
                var existingItem = cart.FirstOrDefault(item => item.ProductId == productId);

                if (existingItem != null)
                {
                    existingItem.Quantity += quantity;
                }
                else
                {
                    cart.Add(new CartItem
                    {
                        ProductId = productId,
                        ProductName = productName,
                        Price = price,
                        ImageUrl = imageUrl,
                        Quantity = quantity
                    });
                }

                SaveCart(cart);
                return Json(new { success = true, count = cart.Sum(item => item.Quantity) });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult UpdateCart(int productId, int quantity)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(i => i.ProductId == productId);

            if (item != null)
            {
                if (quantity <= 0)
                {
                    cart.Remove(item);
                }
                else
                {
                    item.Quantity = quantity;
                }

                SaveCart(cart);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int productId)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(i => i.ProductId == productId);

            if (item != null)
            {
                cart.Remove(item);
                SaveCart(cart);
                TempData["SuccessMessage"] = "Product removed from cart";
            }

            return RedirectToAction("Index");
        }

        public IActionResult GetCartCount()
        {
            var cart = GetCart();
            var count = cart.Sum(item => item.Quantity);
            return Content(count.ToString());
        }

        [HttpPost]
        public IActionResult ClearCart()
        {
            HttpContext.Session.Remove(CartSessionKey);
            TempData["SuccessMessage"] = "Cart cleared successfully";
            return RedirectToAction("Index");
        }
    }
}