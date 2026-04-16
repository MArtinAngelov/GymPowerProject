using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GymPower.Data;
using GymPower.Models;
using GymPower.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GymPower.Tests.Services
{
    public class RecommendationServiceTests
    {
        private async Task<AppDbContext> GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var databaseContext = new AppDbContext(options);
            databaseContext.Database.EnsureCreated();

            return databaseContext;
        }

        [Fact]
        public async Task GetRecommendedProductsForUserAsync_GuestUser_ReturnsRandomAvailableProducts()
        {
            // Arrange
            var dbContext = await GetDatabaseContext();
            dbContext.Products.Add(new Product { Id = 1, Name = "Whey", Category = "Protein", Price = 20, StockQuantity = 10 });
            dbContext.Products.Add(new Product { Id = 2, Name = "Creatine", Category = "Supplements", Price = 15, StockQuantity = 0 }); // Out of stock
            dbContext.Products.Add(new Product { Id = 3, Name = "Shirt", Category = "Clothing", Price = 30, StockQuantity = 5 });
            await dbContext.SaveChangesAsync();

            var service = new RecommendationService(dbContext);

            // Act
            var results = await service.GetRecommendedProductsForUserAsync(null, 2);

            // Assert
            Assert.NotEmpty(results);
            Assert.True(results.Count <= 2);
            Assert.DoesNotContain(results, p => p.Id == 2); // Exclude out of stock
        }

        [Fact]
        public async Task GetRecommendedProductsForUserAsync_RegisteredUser_MatchesGoalKeywords()
        {
            // Arrange
            var dbContext = await GetDatabaseContext();
            dbContext.Users.Add(new User { Id = 1, Username = "TestUser", FitnessGoal = "Build Muscle Protein" });
            
            // Goal Product
            dbContext.Products.Add(new Product { Id = 1, Name = "Whey", Category = "Protein", Price = 20, StockQuantity = 10 });
            dbContext.Products.Add(new Product { Id = 2, Name = "Bar", Category = "Snacks", Price = 5, StockQuantity = 10 });
            await dbContext.SaveChangesAsync();

            var service = new RecommendationService(dbContext);

            // Act
            var results = await service.GetRecommendedProductsForUserAsync(1, 2);

            // Assert
            Assert.NotEmpty(results);
            Assert.Contains(results, p => p.Id == 1); // Should match 'Protein' keyword from goal
        }

        [Fact]
        public async Task GetFrequentlyBoughtTogetherAsync_EmptyCart_ReturnsEmptyList()
        {
            // Arrange
            var dbContext = await GetDatabaseContext();
            var service = new RecommendationService(dbContext);

            // Act
            var results = await service.GetFrequentlyBoughtTogetherAsync(new List<string>());

            // Assert
            Assert.Empty(results);
        }

        [Fact]
        public async Task GetFrequentlyBoughtTogetherAsync_WithCartCategory_ReturnsProductsFromOtherCategories()
        {
            // Arrange
            var dbContext = await GetDatabaseContext();
            dbContext.Products.Add(new Product { Id = 1, Name = "Whey", Category = "Protein", Price = 20, StockQuantity = 10 });
            dbContext.Products.Add(new Product { Id = 2, Name = "Belt", Category = "Accessories", Price = 15, StockQuantity = 5 });
            dbContext.Products.Add(new Product { Id = 3, Name = "BCAAs", Category = "Supplements", Price = 20, StockQuantity = 10 });
            await dbContext.SaveChangesAsync();

            var service = new RecommendationService(dbContext);

            // Act
            var results = await service.GetFrequentlyBoughtTogetherAsync(new List<string> { "Protein" }, 2);

            // Assert
            Assert.NotEmpty(results);
            Assert.True(results.Count <= 2);
            Assert.DoesNotContain(results, p => p.Category == "Protein"); // Should not recommend from same category in cart
        }
    }
}
