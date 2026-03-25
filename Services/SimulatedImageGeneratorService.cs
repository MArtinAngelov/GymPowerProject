using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using GymPower.Models;
using Microsoft.AspNetCore.Hosting;

namespace GymPower.Services
{
    public class SimulatedImageGeneratorService : IProductImageGeneratorService
    {
        private readonly IWebHostEnvironment _env;

        public SimulatedImageGeneratorService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<List<string>> GenerateVariationsAsync(Product product)
        {
            var generatedPaths = new List<string>();
            var uploadsFolder = Path.Combine(_env.WebRootPath, "products");
            
            if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

            // Scenarios requested by user
            var scenarios = new[] { "side_angle", "closeup_label", "gym_environment", "lifestyle_usage", "dramatic_lighting" };
            var safeProductName = string.IsNullOrWhiteSpace(product.Name) ? "product" : product.Name.Replace(" ", "").ToLower();

            // Use distinct placeholder images so the simulation actually looks like different variations!
            var dummySources = new[] 
            {
                "/products/whey_studio_angle.png",
                "/products/whey_closeup_label.png",
                "/products/whey_gym_environment.png",
                "/products/whey_lifestyle_person.png",
                product.ImageUrl // original as 5th
            };

            for (int i = 0; i < scenarios.Length; i++)
            {
                // Format: productname_1.png, productname_2.png...
                string fileName = $"{safeProductName}_{i + 1}.png";
                string fullPath = Path.Combine(uploadsFolder, fileName);

                string sourceImage = i < dummySources.Length ? dummySources[i] : dummySources[0];
                if (string.IsNullOrEmpty(sourceImage) || !sourceImage.StartsWith("/products/"))
                {
                    sourceImage = "/products/wheyprotein.png"; // fallback
                }

                string absoluteSource = Path.Combine(_env.WebRootPath, sourceImage.TrimStart('/'));

                if (File.Exists(absoluteSource))
                {
                    File.Copy(absoluteSource, fullPath, overwrite: true);
                }
                else
                {
                    // If source fails, copy default-product.jpg as fallback natively so it's not a broken text file
                    string defaultFallback = Path.Combine(_env.WebRootPath, "images", "default-product.jpg");
                    if (File.Exists(defaultFallback)) 
                    {
                        File.Copy(defaultFallback, fullPath, overwrite: true);
                    }
                }

                generatedPaths.Add($"/products/{fileName}");
            }

            // Simulated fast execution for bulk generation
            await Task.CompletedTask;

            return generatedPaths;
        }
    }
}
