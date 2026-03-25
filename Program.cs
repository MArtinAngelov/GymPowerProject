using GymPower.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient<GymPower.Services.FreeAIService>();
builder.Services.AddScoped<GymPower.Services.RecommendationService>();
builder.Services.AddScoped<GymPower.Services.InsightsService>();
builder.Services.AddScoped<GymPower.Services.GoalSelectorService>();
builder.Services.AddHttpClient<GymPower.Services.IProductImageGeneratorService, GymPower.Services.PollinationsImageGeneratorService>();


// ✅ Database connection
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// ✅ Session + Context Access
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(6);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<GymPower.Data.AppDbContext>();
    
    // ✅ Auto-migrate database on startup
    db.Database.Migrate();
    
    // Optional if you want: db.Database.EnsureCreated();
    StartupDbPatcher.FixOrdersTableSchema(db);
}

// ✅ Error page
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    DbInitializer.Seed(db);

    // Auto-generate AI variations
    var allProducts = db.Products.Include(p => p.Images).ToList();
    var productsToProcess = allProducts.Where(p => p.Images == null || p.Images.Count < 3).ToList();
    
    // Ако за продукта вече има 3 валидни изображения, пропусни го. (already handled by the Where clause above)
    if (productsToProcess.Any())
    {
        Console.WriteLine($"[AI Generator] Found {productsToProcess.Count} products requiring images. Starting generation...");
        var imageService = scope.ServiceProvider.GetRequiredService<GymPower.Services.IProductImageGeneratorService>();
        
        var productImagesToInsert = new System.Collections.Generic.List<GymPower.Models.ProductImage>();
        int count = 1;
        
        foreach (var p in productsToProcess)
        {
            Console.WriteLine($"[{count}/{productsToProcess.Count}] Generating 3 images for '{p.Name}'...");
            
            if (p.Images != null && p.Images.Any())
            {
                db.ProductImages.RemoveRange(p.Images);
                db.SaveChanges();
            }

            var urls = imageService.GenerateVariationsAsync(p).GetAwaiter().GetResult();
            
            foreach (var url in urls)
            {
                productImagesToInsert.Add(new GymPower.Models.ProductImage { ProductId = p.Id, ImageUrl = url });
            }
            
            // Save immediately per product. This way if the app stops, it resumes from the next product!
            db.ProductImages.AddRange(productImagesToInsert);
            db.SaveChanges();
            productImagesToInsert.Clear();
            
            count++;
        }
    }

    Console.WriteLine($"\n✅ Процесът приключи. Потвърждение: Обектният генератор провери всички {allProducts.Count} продукта.");
}

app.Run();