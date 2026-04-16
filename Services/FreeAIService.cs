using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GymPower.Constants;
using GymPower.Data;
using GymPower.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace GymPower.Services
{
    public class FreeAIService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;
        private readonly ILogger<FreeAIService> _logger;

        public FreeAIService(HttpClient httpClient, IConfiguration configuration, AppDbContext context, ILogger<FreeAIService> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _context = context;
            _logger = logger;
        }

        private string GetNativeFinalUrl()
        {
            var apiKey = _configuration["AiSettings:ApiKey"];
            var model = _configuration["AiSettings:Model"] ?? "gemini-2.5-flash";
            return $"https://generativelanguage.googleapis.com/v1beta/models/{model}:generateContent?key={apiKey}";
        }

        public async Task<FitnessPlan> GetFitnessPlanAsync(User user)
        {
            try
            {
                var products = await _context.Products.AsNoTracking().ToListAsync();
                var productContext = string.Join("\n", products.Select(p => $"{p.Id}: {p.Name} ({p.Category}) - {p.Price:C}"));

                var prompt = $@"
                    Generate a personalized fitness plan for:
                    - Age: {user.Age}
                    - Gender: {user.Gender}
                    - Level: {user.TrainingLevel}
                    - Goal: {user.FitnessGoal}

                    Output strictly valid JSON with this structure:
                    {{
                        ""WeeklySplit"": ""Detailed weekly workout schedule..."",
                        ""NutritionAdvice"": ""Specific daily nutrition tips..."",
                        ""RecommendedProductIds"": [1, 5, 10]
                    }}

                    Product List:
                    {productContext}
                ";

                var requestBody = new
                {
                    systemInstruction = new { parts = new[] { new { text = "You are a fitness expert. Output JSON only." } } },
                    contents = new[]
                    {
                        new { role = "user", parts = new[] { new { text = prompt } } }
                    },
                    generationConfig = new { maxOutputTokens = 1500, responseMimeType = "application/json" }
                };

                _httpClient.DefaultRequestHeaders.Authorization = null;
                var finalUrl = GetNativeFinalUrl();
                
                var response = await _httpClient.PostAsJsonAsync(finalUrl, requestBody);
                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadFromJsonAsync<JsonElement>();
                var content = jsonResponse.GetProperty("candidates")[0].GetProperty("content").GetProperty("parts")[0].GetProperty("text").GetString();

                if (string.IsNullOrEmpty(content)) return new FitnessPlan();
                
                return JsonSerializer.Deserialize<FitnessPlan>(content) ?? new FitnessPlan();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating fitness plan");
                return new FitnessPlan 
                { 
                    WeeklySplit = "Error generating plan.", 
                    NutritionAdvice = "Please try again later." 
                };
            }
        }

        public async Task<string> GetAIResponseAsync(string userMessage, int userId, List<ChatMessage>? guestHistory = null)
        {
            try
            {
                var apiKey = _configuration["AiSettings:ApiKey"];
                var staticPrompt = _configuration["AiSettings:SystemPrompt"] ?? "You are a helpful assistant.";

                if (string.IsNullOrWhiteSpace(apiKey) || apiKey.Contains("PASTE_KEY_HERE"))
                {
                    _logger.LogWarning($"API Key is missing or invalid.");
                    return ChatConstants.ErrorMissingKey;
                }

                if (string.IsNullOrWhiteSpace(userMessage))
                {
                    return ChatConstants.ErrorEmptyMessage;
                }

                var products = await _context.Products.AsNoTracking().Where(p => p.StockQuantity > 0)
                    .Select(p => new { p.Name, p.Category, p.Price, p.Description }).ToListAsync();

                var productContext = new StringBuilder();
                productContext.AppendLine("Product context:");
                foreach (var p in products)
                {
                    productContext.AppendLine($"- {p.Name} ({p.Category}): {p.Price:C}. {p.Description}");
                }

                var fullSystemPrompt = $"{staticPrompt}\n\n{productContext}";

                var allMessages = new List<(string role, string text)>();

                if (userId > 0)
                {
                    var pastMessages = await _context.ChatMessages.AsNoTracking()
                        .Where(m => m.UserId == userId).OrderByDescending(m => m.CreatedAt).Take(10).ToListAsync();
                    pastMessages.Reverse();
                    foreach (var m in pastMessages) allMessages.Add(((m.Role.ToLower() == "assistant" || m.Role.ToLower() == "model") ? "model" : "user", m.Message));
                }
                else if (guestHistory != null)
                {
                    foreach (var m in guestHistory) allMessages.Add(((m.Role.ToLower() == "assistant" || m.Role.ToLower() == "model") ? "model" : "user", m.Message));
                }

                allMessages.Add(("user", userMessage));

                var cleanedMessages = new List<(string role, string text)>();
                string expectedRole = "user";
                foreach (var m in allMessages)
                {
                    if (m.role == expectedRole)
                    {
                        cleanedMessages.Add(m);
                        expectedRole = expectedRole == "user" ? "model" : "user";
                    }
                    else if (m.role == "user" && cleanedMessages.Count > 0 && cleanedMessages.Last().role == "user")
                    {
                        // Merge adjacent user messages to satisfy strictly alternating array if needed
                        var last = cleanedMessages.Last();
                        cleanedMessages[cleanedMessages.Count - 1] = ("user", last.text + "\n" + m.text);
                    }
                }

                if (cleanedMessages.Count == 0 || cleanedMessages.Last().role != "user")
                {
                     cleanedMessages.Add(("user", userMessage)); 
                }

                var processedMessages = new List<object>();
                foreach (var m in cleanedMessages)
                {
                     processedMessages.Add(new { role = m.role, parts = new[] { new { text = m.text } } });
                }

                var requestBody = new
                {
                    systemInstruction = new { parts = new[] { new { text = fullSystemPrompt } } },
                    contents = processedMessages.ToArray(),
                    generationConfig = new { temperature = 0.7, maxOutputTokens = 2048 }
                };

                _httpClient.DefaultRequestHeaders.Authorization = null;
                var finalUrl = GetNativeFinalUrl();

                var response = await _httpClient.PostAsJsonAsync(finalUrl, requestBody);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"AI API Error: {response.StatusCode} - {error}");
                    return $"Грешка от доставчика ({response.StatusCode}): {error}";
                }

                var jsonResponse = await response.Content.ReadFromJsonAsync<JsonElement>();
                if (jsonResponse.TryGetProperty("candidates", out var candidates) && candidates.GetArrayLength() > 0)
                {
                    var content = candidates[0].GetProperty("content").GetProperty("parts")[0].GetProperty("text").GetString();
                    return CleanText(content);
                }

                return "Не получих отговор от асистента.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in FreeAIService.GetAIResponseAsync");
                return ChatConstants.ErrorServiceUnavailable;
            }
        }

        private string CleanText(string? input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;
            var cleaned = Regex.Replace(input, @"[\*`\#]", ""); 
            return cleaned.Trim();
        }
    }
}
