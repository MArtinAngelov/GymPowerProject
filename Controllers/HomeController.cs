using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GymPower.Data;
using GymPower.Models;
using System.Diagnostics;

namespace GymPower.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly GymPower.Services.RecommendationService _recommendationService;

        public HomeController(AppDbContext context, IHttpContextAccessor httpContextAccessor, GymPower.Services.RecommendationService recommendationService)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _recommendationService = recommendationService;
        }

        public async Task<IActionResult> Index()
        {
            var featuredProducts = await _context.Products.Take(3).ToListAsync();
            
            // ✅ Smart Personal Offers
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId.HasValue)
            {
               var personalOffers = await _recommendationService.GetRecommendedProductsForUserAsync(userId, 4);
               ViewBag.PersonalOffers = personalOffers;
            }

            return View(featuredProducts);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}