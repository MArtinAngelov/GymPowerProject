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
using GymPower.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace GymPower.Services
{
    public class AIService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;
        private readonly ILogger<AIService> _logger;

        public AIService(HttpClient httpClient, IConfiguration configuration, AppDbContext context, ILogger<AIService> logger)
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
                var apiKey = _configuration["AISettings:ApiKey"];
                var model = _configuration["AISettings:Model"] ?? "gpt-4o-mini";

                // Check for empty or placeholder keys
                if (string.IsNullOrWhiteSpace(apiKey) || apiKey.Contains("PASTE_KEY_HERE") || apiKey.Contains("ТВОЯ_API_KEY"))
                {
                    _logger.LogWarning($"OpenAI API Key is missing or invalid: '{apiKey}'");
                    return "Грешка: Липсва валиден API Key. Моля, проверете appsettings.json.";
                }

                if (string.IsNullOrWhiteSpace(userMessage))
                {
                    return "Моля, въведете въпрос.";
                }

                // 2. Build Context (Products)
                // Fetch relevant product data to inject into the prompt
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

                // 3. System Prompt
                var systemPrompt = $@"
Ти си професионален фитнес консултант на онлайн магазин GymPower.
Твоята цел е да помагаш на клиентите с фитнес съвети, хранителни режими и ПРЕПОРЪКА НА ПРОДУКТИ от нашия магазин.
Правила:
1. Отговаряй САМО на български език.
2. Бъди учтив, мотивиращ и кратък.
3. Използвай предоставения списък с продукти, за да даваш конкретни предложения.
4. Не си измисляй продукти, които не са в списъка.
5. Ако въпросът не е свързан с фитнес, спорт или здраве, учтиво откажи да отговориш.
6. Не давай медицински съвети.
7. Не използвай Markdown форматиране (bold, italic, list), пиши чист текст.

{productContext}
";

                // 4. Request Payload
                var requestBody = new
                {
                    model = model,
                    messages = new[]
                    {
                        new { role = "system", content = systemPrompt },
                        new { role = "user", content = userMessage }
                    },
                    max_tokens = 500,
                    temperature = 0.7
                };

                // 5. Send Request
                _httpClient.DefaultRequestHeaders.Authorization = 
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);

                var response = await _httpClient.PostAsJsonAsync("https://api.openai.com/v1/chat/completions", requestBody);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"OpenAI API Error: {response.StatusCode} - {error}");
                    return $"Извинете, системата временно не отговаря. (Грешка: {response.StatusCode})";
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
                _logger.LogError(ex, "Exception in GetAIResponseAsync");
                return "Извинете, възникна техническа грешка. Моля, опитайте по-късно.";
            }
        }

        private string CleanText(string? input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;

            // Remove Markdown symbols commonly used by LLMs even when asked not to
            var cleaned = Regex.Replace(input, @"[\*\_`\#]", ""); 
            return cleaned.Trim();
        }
    }
}
