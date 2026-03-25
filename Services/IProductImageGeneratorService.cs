using System.Collections.Generic;
using System.Threading.Tasks;
using GymPower.Models;

namespace GymPower.Services
{
    public interface IProductImageGeneratorService
    {
        /// <summary>
        /// Generates multiple AI variations of a given product image.
        /// Returns a list of the generated file paths.
        /// </summary>
        Task<List<string>> GenerateVariationsAsync(Product product);
    }
}
