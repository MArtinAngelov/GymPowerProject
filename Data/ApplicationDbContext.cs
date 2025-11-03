using Microsoft.EntityFrameworkCore;
using GymPower.Models;

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ✅ Seed Products in Bulgarian
            modelBuilder.Entity<Product>().HasData(
                // Изграждане на мускулна маса
                new Product { Id = 1, Name = "Суроватъчен протеин", Description = "Класически протеин за покачване на чиста мускулна маса и възстановяване.", Price = 69.99m, ImageUrl = "/products/wheyprotein.png", Category = "Изграждане на мускулна маса", StockQuantity = 50 },
                new Product { Id = 2, Name = "Mass Gainer", Description = "Формула с високо съдържание на калории и въглехидрати за бързо покачване на тегло.", Price = 84.90m, ImageUrl = "/products/massgainer.png", Category = "Изграждане на мускулна маса", StockQuantity = 30 },
                new Product { Id = 3, Name = "BCAA Complex", Description = "Аминокиселини с разклонена верига за по-добра мускулна регенерация.", Price = 44.90m, ImageUrl = "/products/BCAAcomplex.png", Category = "Изграждане на мускулна маса", StockQuantity = 60 },
                new Product { Id = 4, Name = "Casein Protein", Description = "Бавноусвоим протеин, идеален преди сън за дълготрайно възстановяване.", Price = 59.90m, ImageUrl = "/products/gaseinprotein.png", Category = "Изграждане на мускулна маса", StockQuantity = 40 },

                // Отслабване
                new Product { Id = 5, Name = "Fat Burner", Description = "Термогенна формула, която подпомага изгарянето на мазнините и повишава енергията.", Price = 54.90m, ImageUrl = "/products/FatBurner.png", Category = "Отслабване", StockQuantity = 70 },
                new Product { Id = 6, Name = "L-Carnitine", Description = "Аминокиселина, която насърчава използването на мазнините като източник на енергия.", Price = 39.99m, ImageUrl = "/products/L-Carnitine.png", Category = "Отслабване", StockQuantity = 80 },
                new Product { Id = 7, Name = "Detox Formula", Description = "Комбинация от билки и антиоксиданти за пречистване и метаболитен баланс.", Price = 34.99m, ImageUrl = "/products/DetoxFormula.png", Category = "Отслабване", StockQuantity = 45 },

                // Регенерация
                new Product { Id = 8, Name = "Glutamine Recovery", Description = "Подпомага мускулното възстановяване и намалява умората след тренировка.", Price = 46.90m, ImageUrl = "/products/GlumatineRecovery.png", Category = "Регенерация", StockQuantity = 55 },
                new Product { Id = 9, Name = "Omega-3", Description = "Рибено масло с високо съдържание на EPA и DHA за стави и сърдечно здраве.", Price = 29.99m, ImageUrl = "/products/Omega-3.png", Category = "Регенерация", StockQuantity = 90 },
                new Product { Id = 10, Name = "Collagen Boost", Description = "Формула за по-здрави стави, сухожилия и кожа.", Price = 49.99m, ImageUrl = "/products/CollagenBoost.png", Category = "Регенерация", StockQuantity = 40 },

                // Висока производителност
                new Product { Id = 11, Name = "Energy Rush", Description = "Мощна енергийна напитка за експлозивни тренировки и издръжливост.", Price = 35.90m, ImageUrl = "/products/energyRush.png", Category = "Висока производителност", StockQuantity = 70 },
                new Product { Id = 12, Name = "Pump Formula", Description = "Аргинин и цитрулин за максимален мускулен напомпващ ефект.", Price = 42.90m, ImageUrl = "/products/pumpFormula.png", Category = "Висока производителност", StockQuantity = 65 },
                new Product { Id = 13, Name = "Focus Shot", Description = "Формула за концентрация и енергия без срив.", Price = 27.90m, ImageUrl = "/products/FocusShot.png", Category = "Висока производителност", StockQuantity = 100 },

                // Имунитет
                new Product { Id = 14, Name = "Vitamin B-3", Description = "Витамини от група B за енергия и имунна подкрепа.", Price = 24.99m, ImageUrl = "/products/VitaminB-3.png", Category = "Имунитет", StockQuantity = 85 },
                new Product { Id = 15, Name = "Multivitamin", Description = "Комплекс от витамини и минерали за всекидневна защита.", Price = 39.90m, ImageUrl = "/products/Multivitamin.png", Category = "Имунитет", StockQuantity = 120 },

                // Здравословна закуска
                new Product { Id = 16, Name = "Protein Bar Box", Description = "Протеинови барчета с високо съдържание на белтъчини и фибри.", Price = 49.90m, ImageUrl = "/products/ProteinBarBox.png", Category = "Здравословна закуска", StockQuantity = 75 },
                new Product { Id = 17, Name = "Oats", Description = "Натурални овесени ядки за енергийна закуска.", Price = 19.99m, ImageUrl = "/products/Oats.png", Category = "Здравословна закуска", StockQuantity = 150 },
                new Product { Id = 18, Name = "Peanut Butter", Description = "Фъстъчено масло без захар – източник на здравословни мазнини и протеин.", Price = 29.90m, ImageUrl = "/products/PeanutButter.png", Category = "Здравословна закуска", StockQuantity = 110 },

                // Облекло
                new Product
                {
                    Id = 19,
                    Name = "GymPower Тениска",
                    Description = "Спортна тениска от дишаща материя, идеална за тренировки.",
                    Price = 39.90m,
                    ImageUrl = "/images/products/PerformanceT-Shirtblack.png",
                    Category = "Облекло",
                    StockQuantity = 40
                },
                new Product { Id = 20, Name = "GymPower Суичър", Description = "Топъл и стилен суичър за спорт и свободно време.", Price = 69.90m, ImageUrl = "/products/GymPowerHodie.png", Category = "Облекло", StockQuantity = 35 },
                new Product { Id = 21, Name = "GymPower Ръкавици", Description = "Удобни фитнес ръкавици за по-добър захват и защита на дланите.", Price = 24.90m, ImageUrl = "/products/gympowergloves.png", Category = "Облекло", StockQuantity = 70 },

                new Product { Id = 22, Name = "GymPower Ластици", Description = "Комплект тренировъчни ластици с различна сила на съпротивление.", Price = 29.90m, ImageUrl = "/products/gympowerband.png", Category = "Облекло", StockQuantity = 60 },
             
                new Product { Id = 23, Name = "True Whey Protein", Description = "Висококачествен суроватъчен протеин за растеж и възстановяване.", Price = 59.99m, ImageUrl = "/products/TrueWheyProtein.png", Category = "Изграждане на мускулна маса", StockQuantity = 60 },
       
                new Product { Id = 24, Name = "Hydro Whey Zero", Description = "Хидролизиран протеин без мазнини и захар за чисти резултати.", Price = 64.99m, ImageUrl = "/products/HydroWheyZero.png", Category = "Изграждане на мускулна маса", StockQuantity = 55 },
        
                new Product { Id = 25, Name = "Pure IsoWhey", Description = "Изолат с 90% протеин за бързо възстановяване след интензивни тренировки.", Price = 69.99m, ImageUrl = "/products/PureIsoWhey.png", Category = "Изграждане на мускулна маса", StockQuantity = 50 },
       
                new Product { Id = 26, Name = "Micellar Casein", Description = "Бавноусвоим протеин, който поддържа възстановяването по време на сън.", Price = 49.99m, ImageUrl = "/products/MiccelarCasein.png", Category = "Регенерация", StockQuantity = 40 },
        
                new Product { Id = 27, Name = "Creatine Monohydrate Creapure", Description = "Чист креатин монохидрат за сила, обем и издръжливост.", Price = 29.99m, ImageUrl = "/products/CreatineMonohydrateCreapure.png", Category = "Регенерация", StockQuantity = 80 },
       
                new Product { Id = 28, Name = "BCAA Instant", Description = "Аминокиселини с бързо усвояване за защита и растеж на мускулите.", Price = 34.99m, ImageUrl = "/products/BCAAInstant.png", Category = "Аминокиселини", StockQuantity = 70 },
        
                new Product { Id = 29, Name = "EAA Mega Stack", Description = "Пълен спектър есенциални аминокиселини за оптимална регенерация.", Price = 39.99m, ImageUrl = "/products/EAAMegaStack.png", Category = "Аминокиселини", StockQuantity = 65 },
        
                new Product { Id = 30, Name = "Citrulline Malate Pump", Description = "Подобрява кръвообращението и напомпването по време на тренировка.", Price = 27.99m, ImageUrl = "/products/CitrullineMalatePump.png", Category = "Предтренировъчни", StockQuantity = 60 },
       
                new Product { Id = 31, Name = "Arginine Alpha-KetoGlutarate (AAKG)", Description = "Аргинин за повече сила и издръжливост.", Price = 25.99m, ImageUrl = "/products/ArginineAlpha-KetoGlutarate(AAKG).png", Category = "Аминокиселини", StockQuantity = 50 },
        
                new Product { Id = 32, Name = "Focus Power Formula", Description = "Формула за ментален фокус и устойчивост по време на тренировка.", Price = 33.99m, ImageUrl = "/products/FocusPowerFormula.png", Category = "Предтренировъчни", StockQuantity = 40 },
       
                new Product { Id = 33, Name = "Pump 3D", Description = "Бустер за по-добър мускулен обем и кръвоснабдяване.", Price = 36.99m, ImageUrl = "/products/Pump3D.png", Category = "Предтренировъчни", StockQuantity = 55 },
       
                new Product { Id = 34, Name = "Black Blood NO Booster", Description = "Мощен бустер с кофеин и бета-аланин за експлозивна сила.", Price = 44.99m, ImageUrl = "/products/BlackBloodNoBooster.png", Category = "Предтренировъчни", StockQuantity = 45 },
        
                new Product { Id = 35, Name = "Nitro Shot Energy", Description = "Предтренировъчна формула за моментален прилив на енергия.", Price = 32.99m, ImageUrl = "/products/NitroShotEnergy.png", Category = "Предтренировъчни", StockQuantity = 65 },
       
                new Product { Id = 36, Name = "Thor PreWorkout", Description = "Експлозивен бустер за сила, енергия и фокус.", Price = 39.99m, ImageUrl = "/products/ThorPreWorkout.png", Category = "Предтренировъчни", StockQuantity = 70 },
        
                new Product { Id = 37, Name = "Vitamin C 1000mg", Description = "Поддържа имунната система и възстановяването след натоварване.", Price = 14.99m, ImageUrl = "/products/VitaminC1000mg.png", Category = "Имунитет", StockQuantity = 90 },
        
                new Product { Id = 38, Name = "VitaPink For Women", Description = "Мултивитамини за жени с биотин, цинк и антиоксиданти.", Price = 24.99m, ImageUrl = "/products/VitaPinkForWomen.png", Category = "Витамини и Минерали", StockQuantity = 80 },
       
                new Product { Id = 39, Name = "Zinc + Magnesium Formula", Description = "Подобрява съня, възстановяването и нивата на тестостерон.", Price = 19.99m, ImageUrl = "/products/Zinc+MagnesiumFormula.png", Category = "Витамини и Минерали", StockQuantity = 75 },
       
                new Product { Id = 40, Name = "Multi Complex", Description = "Мултивитаминен комплекс за енергия и здраве.", Price = 22.99m, ImageUrl = "/products/MultiComplex.png", Category = "Витамини и Минерали", StockQuantity = 60 },
        
                new Product { Id = 41, Name = "Gym Towel Microfiber", Description = "Мека микрофибърна кърпа с лого GymPower – подходяща за фитнес.", Price = 14.99m, ImageUrl = "/products/GymTowelMicrofiber.png", Category = "Аксесоари", StockQuantity = 100 },
       
                new Product { Id = 42, Name = "Resistance Band", Description = "Ластици с различно съпротивление за тренировка у дома и във фитнеса.", Price = 29.99m, ImageUrl = "/products/ResistanceBandOrange.png", Category = "Аксесоари", StockQuantity = 90 },
       
       
                new Product { Id = 43, Name = "Training Gloves Pro", Description = "Професионални ръкавици за сигурен захват и защита.", Price = 24.99m, ImageUrl = "/products/TrainingGlovesPro.png", Category = "Аксесоари", StockQuantity = 75 },
        
                new Product { Id = 44, Name = "Lifting Straps", Description = "Каишки за по-добър захват при тежки упражнения.", Price = 19.99m, ImageUrl = "/products/LiftingStraps.png", Category = "Аксесоари", StockQuantity = 70 },
       
                new Product { Id = 45, Name = "Shaker Bottle Elite 700ml", Description = "Шейкър бутилка с вместимост 700 мл и вградено сито.", Price = 9.99m, ImageUrl = "/products/ShakerBottleElite700ml.png", Category = "Аксесоари", StockQuantity = 120 },
        
                new Product { Id = 46, Name = "Protein Bar Deluxe – Chocolate Peanut", Description = "Протеинов бар с вкус на шоколад и фъстъци.", Price = 3.99m, ImageUrl = "/products/ProteinBarDeluxe–ChocolatePeanut.jpg", Category = "Храни и Добавки", StockQuantity = 150 },
       
                new Product { Id = 47, Name = "GymPower Joggers Carbon Grey", Description = "Мъжки спортни панталони с еластична талия и модерен дизайн.", Price = 49.99m, ImageUrl = "/products/JoggersCarbonGrey.png", Category = "Мъжко облекло", StockQuantity = 40 },
       
                new Product { Id = 48, Name = "Performance T-Shirt Black", Description = "Спортна тениска от дишаща материя, идеална за тренировка и ежедневие.", Price = 34.99m, ImageUrl = "/products/PerformanceTShirtBlack.png", Category = "Мъжко облекло", StockQuantity = 50 },
       
                new Product { Id = 49, Name = "Training Tank Top White", Description = "Безръкавна фланелка с лого GymPower – за свобода и комфорт.", Price = 29.99m, ImageUrl = "/products/TrainingTankTopWhite.png", Category = "Мъжко облекло", StockQuantity = 50 },
       
                new Product { Id = 50, Name = "GymPower Hoodie Orange Edition", Description = "Суичър с контрастни оранжеви детайли и лого GymPower.", Price = 54.99m, ImageUrl = "/products/HoodieOrangeEdition.png", Category = "Мъжко облекло", StockQuantity = 35 },
        
                new Product { Id = 51, Name = "Shorts Flex Fit", Description = "Еластични спортни шорти за тренировки и ежедневие.", Price = 32.99m, ImageUrl = "/products/ShortsFlexFit.png", Category = "Мъжко облекло", StockQuantity = 50 },
       
                new Product { Id = 52, Name = "Seamless Leggings Coral", Description = "Безшевни клинове с висока талия за максимален комфорт.", Price = 49.99m, ImageUrl = "/products/SeamlessLeggingsCoral.png", Category = "Дамско облекло", StockQuantity = 45 },
        
                new Product { Id = 53, Name = "Sport Bra Power Fit", Description = "Спортен сутиен с отлична поддръжка и стилен дизайн.", Price = 34.99m, ImageUrl = "/products/SportBraPowerFit.png", Category = "Дамско облекло", StockQuantity = 60 },
       
                new Product { Id = 54, Name = "Crop Top Black Edition", Description = "Модерен къс топ с лого GymPower – комфорт и визия.", Price = 29.99m, ImageUrl = "/products/CropTopBlackEdition.png", Category = "Дамско облекло", StockQuantity = 55 },
       
                new Product { Id = 55, Name = "High Waist Shorts Pink", Description = "Къси дамски панталонки с висока талия и мека материя.", Price = 27.99m, ImageUrl = "/products/HighWaistShortsPink.png", Category = "Дамско облекло", StockQuantity = 45 },
       
                new Product { Id = 56, Name = "Oversized Hoodie Lilac", Description = "Оувърсайз суичър с модерна визия и комфортна кройка.", Price = 59.99m, ImageUrl = "/products/OversizedHoodieLilac.png", Category = "Дамско облекло", StockQuantity = 40 }
                );
        }
    }
}
