using Microsoft.EntityFrameworkCore;
using GymPower.Models;
using System.Collections.Generic;

namespace GymPower.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductVariant> ProductVariants { get; set; }
        public DbSet<UserGoalPreference> UserGoalPreferences { get; set; }
        public DbSet<ProductGoalMapping> ProductGoalMappings { get; set; }
        public DbSet<RecommendedStack> RecommendedStacks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // All seed data has been moved to DbInitializer.cs
            // Keep this method for future schema configurations (e.g. Fluent API)
        }
    }
}
