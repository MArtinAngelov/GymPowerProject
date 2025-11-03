using GymPower.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace GymPower.Data
{
    public static class DbInitializer
    {
        public static void Seed(AppDbContext context)
        {
            context.Database.Migrate();

            if (context.Products.Any()) return; // ✅ Prevent duplicates

            var products = new List<Product>
            {
                // 🧠 Категория: Отслабване
                new Product { Name = "Thermo Burn Max", Description = "Мощен фет бърнър за изгаряне на мазнини и повече енергия.", Price = 49.90M, ImageUrl = "/products/thermo-burn.jpg", Category = "Отслабване", StockQuantity = 30, IsRecommendedForWeightLoss = true },
                new Product { Name = "L-Carnitine 3000", Description = "Подпомага метаболизма и ускорява редуцирането на мазнини.", Price = 29.90M, ImageUrl = "/products/l_carnitine.jpg", Category = "Отслабване", StockQuantity = 40, IsRecommendedForWeightLoss = true },
                new Product { Name = "CLA Softgels", Description = "Формула с конюгирана линолова киселина за поддържане на форма.", Price = 34.90M, ImageUrl = "/products/cla.jpg", Category = "Отслабване", StockQuantity = 50, IsRecommendedForWeightLoss = true },
                new Product { Name = "Green Tea Extract", Description = "Натурален антиоксидант и метаболитен стимулатор.", Price = 24.90M, ImageUrl = "/products/green_tea.jpg", Category = "Отслабване", StockQuantity = 60 },
                new Product { Name = "Fat Killer Pro", Description = "Интензивна формула за бързо отслабване и тонус.", Price = 54.90M, ImageUrl = "/products/fat_killer.jpg", Category = "Отслабване", StockQuantity = 35 },

                // 🥩 Категория: Изграждане на мускулна маса
                new Product { Name = "Whey Power 100%", Description = "Суроватъчен протеин с отличен вкус и усвояемост.", Price = 59.90M, ImageUrl = "/products/whey_power.jpg", Category = "Изграждане на мускулна маса", StockQuantity = 70, IsRecommendedForMassGain = true },
                new Product { Name = "Mass Gainer Deluxe", Description = "Идеален за изграждане на мускулна маса и сила.", Price = 69.90M, ImageUrl = "/products/mass_gainer.jpg", Category = "Изграждане на мускулна маса", StockQuantity = 40, IsRecommendedForMassGain = true },
                new Product { Name = "Casein Night Protein", Description = "Бавноусвояем протеин за подхранване през нощта.", Price = 49.90M, ImageUrl = "/products/casein.jpg", Category = "Изграждане на мускулна маса", StockQuantity = 50 },
                new Product { Name = "Vegan Protein Blend", Description = "Комбинация от растителни източници за чиста енергия.", Price = 44.90M, ImageUrl = "/products/vegan_protein.jpg", Category = "Изграждане на мускулна маса", StockQuantity = 55 },
                new Product { Name = "Hydro Whey Isolate", Description = "Хидролизиран протеин с бързо усвояване.", Price = 79.90M, ImageUrl = "/products/hydro_whey.jpg", Category = "Изграждане на мускулна маса", StockQuantity = 25 },

                // ⚡ Категория: Висока производителност
                new Product { Name = "Nitro Pump", Description = "Силна предтренировъчна формула за експлозивна енергия.", Price = 39.90M, ImageUrl = "/products/nitro_pump.jpg", Category = "Висока производителност", StockQuantity = 40 },
                new Product { Name = "C4 Ultimate Energy", Description = "Фокус, енергия и мотивация във всяка тренировка.", Price = 49.90M, ImageUrl = "/products/c4.jpg", Category = "Висока производителност", StockQuantity = 30 },
                new Product { Name = "Pump Explosion", Description = "Подобрено кръвоснабдяване и мускулен обем.", Price = 42.90M, ImageUrl = "/products/pump_explosion.jpg", Category = "Висока производителност", StockQuantity = 35 },
                new Product { Name = "No Limits Preworkout", Description = "Бета-аланин, кофеин и цитрулин малат за максимална производителност.", Price = 39.50M, ImageUrl = "/products/no_limits.jpg", Category = "Висока производителност", StockQuantity = 25 },
                new Product { Name = "Power Rush", Description = "Ускорява издръжливостта и концентрацията.", Price = 36.90M, ImageUrl = "/products/power_rush.jpg", Category = "Висока производителност", StockQuantity = 45 },

                // 🧬 Категория: Регенерация
                new Product { Name = "BCAA 2:1:1", Description = "Подпомага мускулното възстановяване и растеж.", Price = 34.90M, ImageUrl = "/products/bcaa.jpg", Category = "Регенерация", StockQuantity = 60, IsRecommendedForMaintenance = true },
                new Product { Name = "EAA Complex", Description = "Пълен спектър есенциални аминокиселини.", Price = 39.90M, ImageUrl = "/products/eaa.jpg", Category = "Регенерация", StockQuantity = 40 },
                new Product { Name = "Glutamine Pure", Description = "Подобрява възстановяването и имунната функция.", Price = 29.90M, ImageUrl = "/products/glutamine_recovery.jpg", Category = "Регенерация", StockQuantity = 50 },
                new Product { Name = "Amino Fuel", Description = "Бързо усвоима формула за сила и възстановяване.", Price = 32.90M, ImageUrl = "/products/amino_fuel.jpg", Category = "Регенерация", StockQuantity = 55 },
                new Product { Name = "BCAA Energy", Description = "Аминокиселини с кофеин за повече енергия.", Price = 37.90M, ImageUrl = "/products/bcaa_energy.jpg", Category = "Регенерация", StockQuantity = 45 },

                // 💊 Категория: Имунитет
                new Product { Name = "Multivitamin Complex", Description = "Пълен спектър витамини и минерали за здраве и тонус.", Price = 24.90M, ImageUrl = "/products/multivitamin.jpg", Category = "Имунитет", StockQuantity = 60 },
                new Product { Name = "Vitamin D3 + K2", Description = "Подсилва имунната система и здравето на костите.", Price = 19.90M, ImageUrl = "/products/vitamin_d3k2.jpg", Category = "Имунитет", StockQuantity = 70 },
                new Product { Name = "Omega-3 Fish Oil", Description = "Поддържа сърдечно-съдовото и мозъчното здраве.", Price = 29.90M, ImageUrl = "/products/omega3.jpg", Category = "Имунитет", StockQuantity = 80 },
                new Product { Name = "Zinc + Magnesium", Description = "Комбинация за по-добро възстановяване и сън.", Price = 22.90M, ImageUrl = "/products/zn_mg.jpg", Category = "Имунитет", StockQuantity = 50 },
                new Product { Name = "Vitamin C Immune", Description = "Антиоксидантна подкрепа и имунна защита.", Price = 17.90M, ImageUrl = "/products/vitamin_c.jpg", Category = "Имунитет", StockQuantity = 100 },

                // 🍎 Категория: Здравословна закуска
                new Product { Name = "Protein Bar Deluxe", Description = "Вкусен протеинов бар за междинно хранене.", Price = 3.90M, ImageUrl = "/products/protein_bar.jpg", Category = "Здравословна закуска", StockQuantity = 120 },
                new Product { Name = "Protein Cookies", Description = "Хрупкави бисквитки с високо съдържание на протеин.", Price = 4.50M, ImageUrl = "/products/protein_cookies.jpg", Category = "Здравословна закуска", StockQuantity = 150 },
                new Product { Name = "Energy Oat Bar", Description = "Богат на въглехидрати бар за енергия през деня.", Price = 2.90M, ImageUrl = "/products/oat_bar.jpg", Category = "Здравословна закуска", StockQuantity = 200 },
                new Product { Name = "Nut Butter Spread", Description = "Фъстъчено масло с високо съдържание на протеин.", Price = 11.90M, ImageUrl = "/products/nut_butter.jpg", Category = "Здравословна закуска", StockQuantity = 90 },
                new Product { Name = "Vegan Protein Chips", Description = "Хрупкав и здравословен снак за след тренировка.", Price = 5.90M, ImageUrl = "/products/protein_chips.jpg", Category = "Здравословна закуска", StockQuantity = 100 },

                // 👕 Категория: Спортно облекло
                new Product { Name = "GymPower Тениска", Description = "Лека, удобна и стилна тениска за тренировка.", Price = 34.90M, ImageUrl = "/products/PerformanceT-Shirtblack.png", Category = "Спортно облекло", StockQuantity = 40 },
                new Product { Name = "Training Shorts", Description = "Спортни шорти от дишаща материя.", Price = 29.90M, ImageUrl = "/products/shorts.jpg", Category = "Спортно облекло", StockQuantity = 35 },
                new Product { Name = "Compression Leggings", Description = "Поддържат мускулите и намаляват умората.", Price = 49.90M, ImageUrl = "/products/leggings.jpg", Category = "Спортно облекло", StockQuantity = 25 },
                new Product { Name = "Gym Hoodie", Description = "Топъл и удобен суитшърт с лого на GymPower.", Price = 59.90M, ImageUrl = "/products/hoodie.jpg", Category = "Спортно облекло", StockQuantity = 30 },
                new Product { Name = "Training Gloves", Description = "Осигуряват стабилен захват и предпазват дланите.", Price = 19.90M, ImageUrl = "/products/gloves.jpg", Category = "Спортно облекло", StockQuantity = 50 },
            };

            context.Products.AddRange(products);
            context.SaveChanges();
        }
    }
}
