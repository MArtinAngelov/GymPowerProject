using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using GymPower.Models;

namespace GymPower.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed data
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Whey Protein Premium",
                    Description = "High-quality whey protein for muscle recovery and growth",
                    Price = 59.99m,
                    ImageUrl = "/images/whey-protein.jpg",
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
                    Description = "Pure creatine for strength and performance",
                    Price = 29.99m,
                    ImageUrl = "/images/creatine.jpg",
                    Category = "Creatine",
                    IsRecommendedForMassGain = true,
                    IsRecommendedForWeightLoss = false,
                    IsRecommendedForMaintenance = true,
                    StockQuantity = 75
                },
                new Product
                {
                    Id = 3,
                    Name = "BCAA Amino Acids",
                    Description = "Essential amino acids for muscle recovery",
                    Price = 39.99m,
                    ImageUrl = "/images/bcaa.jpg",
                    Category = "Amino Acids",
                    IsRecommendedForMassGain = true,
                    IsRecommendedForWeightLoss = true,
                    IsRecommendedForMaintenance = true,
                    StockQuantity = 60
                },
                new Product
                {
                    Id = 4,
                    Name = "Fat Burner Pro",
                    Description = "Advanced formula for weight management",
                    Price = 49.99m,
                    ImageUrl = "/images/fat-burner.jpg",
                    Category = "Weight Loss",
                    IsRecommendedForMassGain = false,
                    IsRecommendedForWeightLoss = true,
                    IsRecommendedForMaintenance = false,
                    StockQuantity = 40
                },
                new Product
                {
                    Id = 5,
                    Name = "Multivitamin Complex",
                    Description = "Complete vitamin formula for athletes",
                    Price = 24.99m,
                    ImageUrl = "/images/multivitamin.jpg",
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
                    Description = "Energy boost for intense workouts",
                    Price = 44.99m,
                    ImageUrl = "/images/pre-workout.jpg",
                    Category = "Pre-Workout",
                    IsRecommendedForMassGain = true,
                    IsRecommendedForWeightLoss = false,
                    IsRecommendedForMaintenance = true,
                    StockQuantity = 55
                }
            );

            // Create admin user
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "admin",
                    Email = "admin@gympower.com",
                    Password = "admin123", // In real app, this should be hashed
                    Role = "Admin",
                    FitnessGoal = "Maintenance",
                    CreatedAt = DateTime.Now
                }
            );
        }
    }
}
