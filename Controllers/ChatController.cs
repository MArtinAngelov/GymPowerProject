using System;
using System.Linq;
using System.Threading.Tasks;
using GymPower.Data;
using GymPower.Models;
using GymPower.Services;
using GymPower.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace GymPower.Controllers
{
    public class ChatController : Controller
    {
        private readonly AppDbContext _context;
        private readonly FreeAIService _aiService;

        public ChatController(AppDbContext context, FreeAIService aiService)
        {
            _context = context;
            _aiService = aiService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var messages = await _context.ChatMessages
                .Where(m => m.UserId == userId)
                .OrderBy(m => m.CreatedAt)
                .ToListAsync();

            return View(messages);
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] ChatRequest request)
        {
            try 
            {
                var userId = HttpContext.Session.GetInt32("UserId");
                if (userId == null)
                {
                    return Unauthorized();
                }

                if (string.IsNullOrWhiteSpace(request.Message))
                {
                    return BadRequest("Съобщението не може да бъде празно.");
                }

                // Save User Message
                var userMsg = new ChatMessage
                {
                    UserId = userId.Value,
                    Role = ChatConstants.UserRole,
                    Message = request.Message,
                    CreatedAt = DateTime.Now
                };
                _context.ChatMessages.Add(userMsg);
                await _context.SaveChangesAsync();

                // Get AI Response
                var aiResponse = await _aiService.GetAIResponseAsync(request.Message, userId.Value);

                // Save AI Message
                var aiMsg = new ChatMessage
                {
                    UserId = userId.Value,
                    Role = ChatConstants.AssistantRole,
                    Message = aiResponse,
                    CreatedAt = DateTime.Now
                };
                _context.ChatMessages.Add(aiMsg);
                await _context.SaveChangesAsync();

                return Json(new { response = aiResponse });
            }
            catch (Exception ex)
            {
                // Log exception here if logger was injected
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }

    public class ChatRequest
    {
        public string Message { get; set; }
    }
}
