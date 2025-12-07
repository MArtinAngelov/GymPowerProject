using GymPower.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GymPower.Controllers.Api
{
    [Route("api/validation")]
    [ApiController]
    public class ValidationController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ValidationController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("check-user")]
        public async Task<IActionResult> CheckUser([FromBody] UserCheckRequest request)
        {
            var response = new 
            {
                UsernameAvailable = true,
                EmailAvailable = true
            };

            if (!string.IsNullOrWhiteSpace(request.Username))
            {
                var exists = await _context.Users.AnyAsync(u => u.Username == request.Username);
                if (exists) response = new { UsernameAvailable = false, response.EmailAvailable };
            }

            if (!string.IsNullOrWhiteSpace(request.Email))
            {
                var exists = await _context.Users.AnyAsync(u => u.Email == request.Email);
                if (exists) response = new { response.UsernameAvailable, EmailAvailable = false };
            }

            return Ok(response);
        }
    }

    public class UserCheckRequest
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
    }
}
