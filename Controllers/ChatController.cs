using System;
using System.Linq;
using System.Threading.Tasks;
using GymPower.Data;
using GymPower.Models;
using GymPower.Services;
using GymPower.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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

                if (string.IsNullOrWhiteSpace(request.Message))
                {
                    return BadRequest("Съобщението не може да бъде празно.");
                }

                // If logged in, save User Message
                if (userId.HasValue)
                {
                    var userMsg = new ChatMessage
                    {
                        UserId = userId.Value,
                        Role = ChatConstants.UserRole,
                        Message = request.Message,
                        CreatedAt = DateTime.Now
                    };
                    _context.ChatMessages.Add(userMsg);
                    await _context.SaveChangesAsync();
                }

                List<ChatMessage>? guestHistoryWrapper = null;
                List<ChatMessage> guestHistory = new List<ChatMessage>();

                if (!userId.HasValue)
                {
                    var sessionHistoryJson = HttpContext.Session.GetString("GuestChatHistory");
                    if (!string.IsNullOrEmpty(sessionHistoryJson))
                    {
                        guestHistoryWrapper = System.Text.Json.JsonSerializer.Deserialize<List<ChatMessage>>(sessionHistoryJson);
                    }
                    if (guestHistoryWrapper != null)
                    {
                        guestHistory = guestHistoryWrapper;
                    }
                }

                // Get AI Response (pass 0 for guests, and pass the guest memory list)
                var aiResponse = await _aiService.GetAIResponseAsync(request.Message, userId ?? 0, guestHistory);

                // If logged in, save AI Message
                if (userId.HasValue)
                {
                    var aiMsg = new ChatMessage
                    {
                        UserId = userId.Value,
                        Role = ChatConstants.AssistantRole,
                        Message = aiResponse,
                        CreatedAt = DateTime.Now
                    };
                    _context.ChatMessages.Add(aiMsg);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    // Guest Memory Persistence
                    guestHistory.Add(new ChatMessage { Role = ChatConstants.UserRole, Message = request.Message });
                    guestHistory.Add(new ChatMessage { Role = ChatConstants.AssistantRole, Message = aiResponse });
                    // Keep last 10 messages for guests to avoid blowing up session cookie size
                    if (guestHistory.Count > 10) guestHistory = guestHistory.Skip(guestHistory.Count - 10).ToList();
                    HttpContext.Session.SetString("GuestChatHistory", System.Text.Json.JsonSerializer.Serialize(guestHistory));
                }

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
        public string Message { get; set; } = string.Empty;
    }
}
