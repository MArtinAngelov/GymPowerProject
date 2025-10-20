using Microsoft.EntityFrameworkCore;
using GymPower.Models;

namespace GymPower.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed real product data with images
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Whey Protein Premium",
                    Description = "High-quality whey protein for muscle recovery and growth. 25g protein per serving with essential amino acids for optimal results.",
                    Price = 59.99m,
                    ImageUrl = "/images/products/whey-protein.jpg",
                    Category = "Protein",
                    IsRecommendedForMassGain = true,
                    IsRecommendedForWeightLoss = true,
                    IsRecommendedForMaintenance = true,
                    StockQuantity = 50
                },
                new Product
                {
                    Id = 2,
                    Name = "Creatine Monohydrate",
                    Description = "Pure creatine monohydrate for strength and performance. Increases power output and muscle volume. 100% pure formula.",
                    Price = 29.99m,
                    ImageUrl = "/images/products/creatine.jpg",
                    Category = "Creatine",
                    IsRecommendedForMassGain = true,
                    IsRecommendedForWeightLoss = false,
                    IsRecommendedForMaintenance = true,
                    StockQuantity = 75
                },
                new Product
                {
                    Id = 3,
                    Name = "Fat Burner Pro",
                    Description = "Advanced thermogenic formula for weight management. Supports metabolism and energy levels without jitters.",
                    Price = 49.99m,
                    ImageUrl = "/images/products/fat-burner.jpg",
                    Category = "Weight Loss",
                    IsRecommendedForMassGain = false,
                    IsRecommendedForWeightLoss = true,
                    IsRecommendedForMaintenance = false,
                    StockQuantity = 40
                },
                new Product
                {
                    Id = 4,
                    Name = "BCAA Amino Acids",
                    Description = "Essential branched-chain amino acids for muscle recovery. Reduces muscle soreness and fatigue after workouts.",
                    Price = 39.99m,
                    ImageUrl = "/images/products/bcaa.jpg",
                    Category = "Amino Acids",
                    IsRecommendedForMassGain = true,
                    IsRecommendedForWeightLoss = true,
                    IsRecommendedForMaintenance = true,
                    StockQuantity = 60
                },
                new Product
                {
                    Id = 5,
                    Name = "Multivitamin Complex",
                    Description = "Complete vitamin and mineral formula for athletes. Supports immune function and overall health with 25 essential nutrients.",
                    Price = 24.99m,
                    ImageUrl = "/images/products/multivitamin.jpg",
                    Category = "Vitamins",
                    IsRecommendedForMassGain = true,
                    IsRecommendedForWeightLoss = true,
                    IsRecommendedForMaintenance = true,
                    StockQuantity = 100
                },
                new Product
                {
                    Id = 6,
                    Name = "Pre-Workout Energizer",
                    Description = "Powerful pre-workout formula for energy and focus. Enhances performance and endurance during intense training sessions.",
                    Price = 44.99m,
                    ImageUrl = "/images/products/pre-workout.jpg",
                    Category = "Pre-Workout",
                    IsRecommendedForMassGain = true,
                    IsRecommendedForWeightLoss = false,
                    IsRecommendedForMaintenance = true,
                    StockQuantity = 55
                },
                new Product
                {
                    Id = 7,
                    Name = "Mass Gainer Extreme",
                    Description = "High-calorie mass gainer with complex carbs and protein. Perfect for hardgainers looking to build serious mass.",
                    Price = 69.99m,
                    ImageUrl = "/images/products/mass-gainer.jpg",
                    Category = "Mass Gainer",
                    IsRecommendedForMassGain = true,
                    IsRecommendedForWeightLoss = false,
                    IsRecommendedForMaintenance = false,
                    StockQuantity = 30
                },
                new Product
                {
                    Id = 8,
                    Name = "L-Carnitine 3000",
                    Description = "Advanced L-Carnitine formula for fat transportation. Supports energy production from fats during cardio workouts.",
                    Price = 34.99m,
                    ImageUrl = "/images/products/l-carnitine.jpg",
                    Category = "Weight Loss",
                    IsRecommendedForMassGain = false,
                    IsRecommendedForWeightLoss = true,
                    IsRecommendedForMaintenance = false,
                    StockQuantity = 45
                },
                new Product
                {
                    Id = 9,
                    Name = "Protein Bars (12-pack)",
                    Description = "Delicious protein bars with 20g protein each. Perfect for post-workout recovery or as a healthy snack.",
                    Price = 32.99m,
                    ImageUrl = "/images/products/protein-bar.jpg",
                    Category = "Snacks",
                    IsRecommendedForMassGain = true,
                    IsRecommendedForWeightLoss = true,
                    IsRecommendedForMaintenance = true,
                    StockQuantity = 80
                },
                new Product
                {
                    Id = 10,
                    Name = "Omega-3 Fish Oil",
                    Description = "High-potency fish oil with EPA and DHA. Supports joint health, brain function, and cardiovascular health.",
                    Price = 19.99m,
                    ImageUrl = "/images/products/fish-oil.jpg",
                    Category = "Health",
                    IsRecommendedForMassGain = true,
                    IsRecommendedForWeightLoss = true,
                    IsRecommendedForMaintenance = true,
                    StockQuantity = 120
                }
            );

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "admin",
                    Email = "admin@gympower.com",
                    Password = "admin123",
                    Role = "Admin",
                    FitnessGoal = "Maintenance",
                    CreatedAt = DateTime.Now
                }
            );
        }
    }
}