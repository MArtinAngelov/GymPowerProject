using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using GymPower.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace GymPower.Services
{
    public class PollinationsImageGeneratorService : IProductImageGeneratorService
    {
        private readonly IWebHostEnvironment _env;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public PollinationsImageGeneratorService(IWebHostEnvironment env, HttpClient httpClient, IConfiguration configuration)
        {
            _env = env;
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<List<string>> GenerateVariationsAsync(Product product)
        {
            var generatedPaths = new List<string>();
            var uploadsFolder = Path.Combine(_env.WebRootPath, "images", "products", product.Id.ToString());
            
            if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

            var basePrompt = $"High-quality photo of {product.Name}, a product for {product.Category}. Description: {product.Description}. Style: ";
            var scenarios = new[] 
            {
                basePrompt + "Студиен кадър – акцент върху опаковката и етикета.",
                basePrompt + "Lifestyle снимка – продуктът в реална фитнес среда.",
                basePrompt + "Close-up детайлна снимка – фокус върху текстурата и детайла."
            };

            var apiKey = _configuration["AiSettings:ApiKey"];
            if (string.IsNullOrEmpty(apiKey)) throw new Exception("Together API Key missing!");
            
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);

            for (int i = 0; i < scenarios.Length; i++)
            {
                string fileName = $"image{i + 1}.jpg";
                string fullPath = Path.Combine(uploadsFolder, fileName);

                var requestBody = new
                {
                    model = "black-forest-labs/FLUX.1-schnell",
                    prompt = scenarios[i],
                    width = 1024,
                    height = 1024,
                    steps = 4,
                    n = 1,
                    response_format = "b64_json"
                };

                try 
                {
                    var response = await _httpClient.PostAsJsonAsync("https://api.together.xyz/v1/images/generations", requestBody);
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonDoc = await response.Content.ReadFromJsonAsync<JsonElement>();
                        var dataNode = jsonDoc.GetProperty("data")[0];
                        
                        if (dataNode.TryGetProperty("b64_json", out var b64Prop))
                        {
                            var bytes = Convert.FromBase64String(b64Prop.GetString()!);
                            await File.WriteAllBytesAsync(fullPath, bytes);
                            generatedPaths.Add($"/images/products/{product.Id}/{fileName}");
                        }
                        else if (dataNode.TryGetProperty("url", out var urlProp))
                        {
                            var imageUrl = urlProp.GetString()!;
                            var imageBytes = await _httpClient.GetByteArrayAsync(imageUrl);
                            await File.WriteAllBytesAsync(fullPath, imageBytes);
                            generatedPaths.Add($"/images/products/{product.Id}/{fileName}");
                        }
                    }
                    else 
                    {
                        Console.WriteLine($"[Together API Error on Product {product.Id}] {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to generate image {fileName}: {ex.Message}");
                }
                
                await Task.Delay(2000); // Respect Free Tier limits (1 req/sec)
            }

            return generatedPaths;
        }
    }
}
