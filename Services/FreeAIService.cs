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
                        ""RecommendedProductIds"": [1, 5, 10] // Choose 3 best product IDs from the list below based on the goal.
                    }}

                    Product List:
                    {productContext}
                ";

                var requestBody = new
                {
                    model = _configuration["AiSettings:Model"] ?? "gpt-4o-mini",
                    messages = new[]
                    {
                        new { role = "system", content = "You are a fitness expert. Output JSON only." },
                        new { role = "user", content = prompt }
                    },
                    max_tokens = 1000,
                    response_format = new { type = "json_object" }
                };

                var apiKey = _configuration["AiSettings:ApiKey"];
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);
                
                var response = await _httpClient.PostAsJsonAsync(_configuration["AiSettings:BaseUrl"], requestBody);
                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadFromJsonAsync<JsonElement>();
                var content = jsonResponse.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();

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

        public async Task<string> GetAIResponseAsync(string userMessage, int userId)
        {
            try
            {
                // 1. Load Configuration
                var apiKey = _configuration["AiSettings:ApiKey"];
                var model = _configuration["AiSettings:Model"] ?? "gpt-4o-mini";
                var baseUrl = _configuration["AiSettings:BaseUrl"] ?? "https://api.openai.com/v1/chat/completions";
                var staticPrompt = _configuration["AiSettings:SystemPrompt"] ?? "You are a helpful assistant.";

                // Validate Key
                if (string.IsNullOrWhiteSpace(apiKey) || apiKey.Contains("PASTE_KEY_HERE") || apiKey.Contains("PASTE_TOGETHER_KEY"))
                {
                    _logger.LogWarning($"API Key is missing or invalid.");
                    return ChatConstants.ErrorMissingKey;
                }

                if (string.IsNullOrWhiteSpace(userMessage))
                {
                    return ChatConstants.ErrorEmptyMessage;
                }

                // 2. Build Context (Products)
                var products = await _context.Products
                    .AsNoTracking()
                    .Where(p => p.StockQuantity > 0)
                    .Select(p => new { p.Name, p.Category, p.Price, p.Description })
                    .ToListAsync();

                var productContext = new StringBuilder();
                productContext.AppendLine("Product context:");
                foreach (var p in products)
                {
                    productContext.AppendLine($"- {p.Name} ({p.Category}): {p.Price:C}. {p.Description}");
                }

                // 3. Combine Static Prompt with Dynamic Context
                var fullSystemPrompt = $"{staticPrompt}\n\n{productContext}";

                // 4. Request Payload (Standard OpenAI Compatible)
                var requestBody = new
                {
                    model = model,
                    messages = new[]
                    {
                        new { role = "system", content = fullSystemPrompt },
                        new { role = "user", content = userMessage }
                    },
                    max_tokens = 512,
                    temperature = 0.7
                    // Removed provider-specific params like top_k to ensure cross-compatibility
                };

                // 5. Send Request
                _httpClient.DefaultRequestHeaders.Authorization = 
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);

                var response = await _httpClient.PostAsJsonAsync(baseUrl, requestBody);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"AI API Error: {response.StatusCode} - {error}");
                    
                    var hint = "";
                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        hint = " (Съвет: Уверете се, че 'Generative Language API' е включено във вашия Google Cloud Console проект)";
                    }

                    return $"Грешка от доставчика ({response.StatusCode}): {error}{hint}";
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
