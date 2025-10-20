using Microsoft.EntityFrameworkCore;
using GymPower.Models;

namespace GymPower.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // ONLY include models that go in the database
        public DbSet<User> Users => Set<User>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();

        // REMOVE this line: public DbSet<CartItem> CartItems => Set<CartItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure relationships
            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.ProductId);

            // Seed data
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Whey Protein Premium",
                    Description = "High-quality whey protein for muscle recovery and growth",
                    Price = 59.99m,
                    ImageUrl = "/images/protein.jpg",
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
                    Name = "Fat Burner Pro",
                    Description = "Advanced formula for weight management",
                    Price = 49.99m,
                    ImageUrl = "/images/fatburner.jpg",
                    Category = "Weight Loss",
                    IsRecommendedForMassGain = false,
                    IsRecommendedForWeightLoss = true,
                    IsRecommendedForMaintenance = false,
                    StockQuantity = 40
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