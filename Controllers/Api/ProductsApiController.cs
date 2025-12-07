using GymPower.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace GymPower.Controllers.Api
{
    [Route("api/products")]
    [ApiController]
    public class ProductsApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsApiController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(string term)
        {
            if (string.IsNullOrWhiteSpace(term))
            {
                return Ok(new object[] { });
            }

            var normalized = term.ToLower();

            var products = await _context.Products
                .Where(p => p.Name.ToLower().Contains(normalized) || 
                            p.Category.ToLower().Contains(normalized))
                .Select(p => new {
                    p.Id,
                    p.Name,
                    p.Category,
                    p.ImageUrl,
                    p.Price
                })
                .Take(5) // Limit results
                .ToListAsync();

            return Ok(products);
        }
    }
}
