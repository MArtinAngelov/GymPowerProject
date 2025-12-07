using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GymPower.Data;
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

        public async Task<string> GetAIResponseAsync(string userMessage, int userId)
        {
            try
            {
                // 1. Validate Config
                var apiKey = _configuration["TogetherAiSettings:ApiKey"];
                var model = _configuration["TogetherAiSettings:Model"] ?? "meta-llama/Llama-3-8b-chat-hf";

                if (string.IsNullOrWhiteSpace(apiKey) || apiKey.Contains("PASTE_TOGETHER_KEY_HERE"))
                {
                    _logger.LogWarning($"TogetherAI API Key is missing or placeholder: '{apiKey}'");
                    return "Грешка: Липсва валиден API Key за TogetherAI. Моля, конфигурирайте го в appsettings.json.";
                }

                if (string.IsNullOrWhiteSpace(userMessage))
                {
                    return "Моля, въведете въпрос.";
                }

                // 2. Build Context (Products)
                var products = await _context.Products
                    .AsNoTracking()
                    .Where(p => p.StockQuantity > 0)
                    .Select(p => new { p.Name, p.Category, p.Price, p.Description })
                    .ToListAsync();

                var productContext = new StringBuilder();
                productContext.AppendLine("Налични продукти в магазина:");
                foreach (var p in products)
                {
                    productContext.AppendLine($"- {p.Name} ({p.Category}): {p.Price:C}. {p.Description}");
                }

                // 3. System Prompt for Llama-3
                var systemPrompt = $@"
You are a helpful fitness assistant for the 'GymPower' online store.
Your goal is to help customers with fitness advice and recommend OUR products.
RULES:
1. ALWAYS reply in BULGARIAN language.
2. Use the provided Product List to make specific recommendations.
3. Be polite, concise, and professional.
4. Do NOT recommend products not in the list.
5. Do NOT use markdown formatting.

PRODUCT LIST:
{productContext}
";

                // 4. Request Payload (TogetherAI is OpenAI compatible)
                var requestBody = new
                {
                    model = model,
                    messages = new[]
                    {
                        new { role = "system", content = systemPrompt },
                        new { role = "user", content = userMessage }
                    },
                    max_tokens = 512,
                    temperature = 0.7,
                    top_p = 0.7,
                    top_k = 50,
                    repetition_penalty = 1
                };

                // 5. Send Request
                _httpClient.DefaultRequestHeaders.Authorization = 
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);

                var response = await _httpClient.PostAsJsonAsync("https://api.together.xyz/v1/chat/completions", requestBody);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"TogetherAI API Error: {response.StatusCode} - {error}");
                    return $"Извинете, възникна грешка при връзката с AI услугата. (Status: {response.StatusCode})";
                }

                // 6. Parse Response
                var jsonResponse = await response.Content.ReadFromJsonAsync<JsonElement>();
                
                if (jsonResponse.TryGetProperty("choices", out var choices) && choices.GetArrayLength() > 0)
                {
                    var content = choices[0].GetProperty("message").GetProperty("content").GetString();
                    return CleanText(content);
                }

                return "Не получих отговор от асистента.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in FreeAIService.GetAIResponseAsync");
                return "Извинете, възникна техническа грешка. Моля, опитайте по-късно.";
            }
        }

        private string CleanText(string? input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;
            // Remove markdown
            var cleaned = Regex.Replace(input, @"[\*\_`\#]", ""); 
            return cleaned.Trim();
        }
    }
}
