using GymPower.Data;
using GymPower.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymPower.Controllers
{
    public class ProductsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly GymPower.Services.RecommendationService _recommendationService;
        private readonly Microsoft.AspNetCore.Hosting.IWebHostEnvironment _webHostEnvironment;

        public ProductsController(AppDbContext context, GymPower.Services.RecommendationService recommendationService, Microsoft.AspNetCore.Hosting.IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _recommendationService = recommendationService;
            _webHostEnvironment = webHostEnvironment;
        }

        private async Task<string?> UploadImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0) return null;
            
            var uploadsFolder = System.IO.Path.Combine(_webHostEnvironment.WebRootPath, "products");
            if (!System.IO.Directory.Exists(uploadsFolder)) System.IO.Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + System.IO.Path.GetFileName(file.FileName);
            var filePath = System.IO.Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return "/products/" + uniqueFileName;
        }

        // GET: Products
        public IActionResult Index(string? category, string? search)
        {
            // Load all products first
            var products = _context.Products.ToList();

            // ✅ Filter by category (case-insensitive + trims + Bulgarian letters safe)
            if (!string.IsNullOrWhiteSpace(category))
            {
                string normalized = category.Trim().ToLower();
                products = products
                    .Where(p => p.Category != null &&
                                p.Category.Trim().ToLower().Contains(normalized))
                    .ToList();
            }

            // ✅ Search filter (also safe and case-insensitive)
            if (!string.IsNullOrWhiteSpace(search))
            {
                string normalizedSearch = search.Trim().ToLower();
                products = products
                    .Where(p =>
                        (p.Name != null && p.Name.ToLower().Contains(normalizedSearch)) ||
                        (p.Description != null && p.Description.ToLower().Contains(normalizedSearch)) ||
                        (p.Category != null && p.Category.ToLower().Contains(normalizedSearch))
                    )
                    .ToList();
            }

            return View(products);
        }

        // ✅ Helper: Normalize text (removes accents & softens search)
        private static string NormalizeText(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;
            var normalized = input.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();

            foreach (var c in normalized)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }

            return sb.ToString()
                     .Normalize(NormalizationForm.FormC)
                     .Replace("y", "i") // helps with translit typos
                     .Replace("u", "v")
                     .Replace("q", "k");
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Products
                .Include(p => p.Variants)
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == id);
                
            if (product == null) return NotFound();

            // Load related products from the same category
            var relatedProducts = await _context.Products
                .Where(p => p.Category == product.Category && p.Id != product.Id)
                .Take(4)
                .ToListAsync();

            ViewBag.RelatedProducts = relatedProducts;

            // ✅ Smart Recommendations (Personalized)
            var userId = HttpContext.Session.GetInt32("UserId");
            var recommended = await _recommendationService.GetRecommendedProductsForUserAsync(userId, 4) ?? new List<Product>();
            
            // Filter out current product from recommendations just in case
            ViewBag.RecommendedProducts = recommended.Where(p => p.Id != product.Id).Take(4).ToList();

            return View(product);
        }
        // GET: Products/Create
        public IActionResult Create()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product, IFormFile? MainImageFile, List<IFormFile>? ExtraImageFiles)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {
                // Process Main Image
                if (MainImageFile != null)
                {
                    product.ImageUrl = await UploadImageAsync(MainImageFile) ?? "/images/products/default-product.jpg";
                }
                else if (string.IsNullOrEmpty(product.ImageUrl))
                {
                    product.ImageUrl = "/images/products/default-product.jpg";
                }

                _context.Add(product);
                await _context.SaveChangesAsync();

                // Process Extra Images
                if (ExtraImageFiles != null && ExtraImageFiles.Any())
                {
                    foreach (var file in ExtraImageFiles)
                    {
                        var imageUrl = await UploadImageAsync(file);
                        if (!string.IsNullOrEmpty(imageUrl))
                        {
                            _context.ProductImages.Add(new ProductImage
                            {
                                ProductId = product.Id,
                                ImageUrl = imageUrl
                            });
                        }
                    }
                    await _context.SaveChangesAsync();
                }

                TempData["SuccessMessage"] = "Product created successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            if (id == null) return NotFound();

            var product = await _context.Products
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();
            
            // Populate Extra Images CSV for Admin
            if (product.Images != null && product.Images.Any())
            {
                ViewBag.ExtraImages = string.Join(", ", product.Images.Select(i => i.ImageUrl));
            }

            return View(product);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product, IFormFile? MainImageFile, List<IFormFile>? ExtraImageFiles)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Process Main Image modification (keep old if not modified)
                    if (MainImageFile != null)
                    {
                        var imageUrl = await UploadImageAsync(MainImageFile);
                        if (!string.IsNullOrEmpty(imageUrl))
                        {
                            product.ImageUrl = imageUrl;
                        }
                    }

                    _context.Update(product);

                    // Update Extra Images
                    if (ExtraImageFiles != null && ExtraImageFiles.Any())
                    {
                        // Clear old, Add new (or append depending on logic, but currently logic clears old)
                        var existingImages = await _context.ProductImages.Where(i => i.ProductId == id).ToListAsync();
                        _context.ProductImages.RemoveRange(existingImages);

                        foreach (var file in ExtraImageFiles)
                        {
                            var imageUrl = await UploadImageAsync(file);
                            if (!string.IsNullOrEmpty(imageUrl))
                            {
                                _context.ProductImages.Add(new ProductImage
                                {
                                    ProductId = id,
                                    ImageUrl = imageUrl
                                });
                            }
                        }
                    }

                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Product updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }


        // POST: Products/DeleteImage/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteImage(int imageId, int productId)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            var image = await _context.ProductImages.FindAsync(imageId);
            if (image != null && image.ProductId == productId)
            {
                _context.ProductImages.Remove(image);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Image deleted successfully!";
            }

            return RedirectToAction(nameof(Edit), new { id = productId });
        }


        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Product deleted successfully!";
            }

            return RedirectToAction(nameof(Index));
        }
        public IActionResult Search(string query)
        {
            var results = string.IsNullOrWhiteSpace(query)
                ? _context.Products.ToList()
                : _context.Products
                    .Where(p => p.Name.Contains(query) || p.Description.Contains(query))
                    .ToList();

            return View("Index", results);
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }

        private bool IsAdmin()
        {
            var role = HttpContext.Session.GetString("Role");
            return role == "Admin";
        }
    }
}