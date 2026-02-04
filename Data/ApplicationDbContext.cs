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

            // ✅ Seed Products with LongDescription
            modelBuilder.Entity<Product>().HasData(
                new Product 
                { 
                    Id = 1, 
                    Name = "Суроватъчен протеин", 
                    Description = "Класически протеин за покачване на чиста мускулна маса и възстановяване.", 
                    LongDescription = "Този висококачествен суроватъчен протеин е идеалният избор за всеки спортуващ. Съдържа оптимален профил на аминокиселини, които подпомагат бързото възстановяване след тренировка и стимулират мускулния растеж. Приемайте една доза (30г) сутрин на гладно или веднага след тренировка за най-добри резултати.",
                    Price = 69.99m, 
                    ImageUrl = "/products/wheyprotein.png", 
                    Category = "Изграждане на мускулна маса", 
                    StockQuantity = 50 
                },
                new Product 
                { 
                    Id = 2, 
                    Name = "Mass Gainer", 
                    Description = "Формула с високо съдържание на калории и въглехидрати за бързо покачване на тегло.", 
                    LongDescription = "Mass Gainer е специално разработен за тези, които трудно покачват килограми („хардгейнъри“). Със своята формула, богата на комплексни въглехидрати и висококачествен протеин, той осигурява необходимите калории за изграждане на масивна мускулатура. Идеален за прием след тренировка или като междинно хранене.",
                    Price = 84.90m, 
                    ImageUrl = "/products/massgainer.png", 
                    Category = "Изграждане на мускулна маса", 
                    StockQuantity = 30 
                },
                new Product 
                { 
                    Id = 3, 
                    Name = "BCAA Complex", 
                    Description = "Аминокиселини с разклонена верига за по-добра мускулна регенерация.", 
                    LongDescription = "BCAA Complex предоставя незаменимите аминокиселини Левцин, Изолевцин и Валин в оптимално съотношение 2:1:1. Те предпазват мускулите от разграждане (катаболизъм) по време на интензивни тренировки и ускоряват възстановяването. Приемайте преди, по време или след тренировка.",
                    Price = 44.90m, 
                    ImageUrl = "/products/BCAAcomplex.png", 
                    Category = "Изграждане на мускулна маса", 
                    StockQuantity = 60 
                },
                new Product 
                { 
                    Id = 4, 
                    Name = "Casein Protein", 
                    Description = "Бавноусвоим протеин, идеален преди сън за дълготрайно възстановяване.", 
                    LongDescription = "Мицеларният казеин е протеин с бавно освобождаване, който подхранва мускулите ви в продължение на до 8 часа. Той е перфектният избор за прием преди лягане, осигурявайки постоянен приток на аминокиселини през нощта, когато тялото се възстановява най-активно.",
                    Price = 59.90m, 
                    ImageUrl = "/products/gaseinprotein.png", 
                    Category = "Изграждане на мускулна маса", 
                    StockQuantity = 40 
                },
                new Product 
                { 
                    Id = 5, 
                    Name = "Fat Burner", 
                    Description = "Термогенна формула, която подпомага изгарянето на мазнините и повишава енергията.", 
                    LongDescription = "Нашата мощна термогенна формула ускорява метаболизма и помага на тялото да използва мастните депа като източник на енергия. Съдържа кофеин, зелен чай и Л-карнитин, които не само топят мазнините, но и повишават фокуса и издръжливостта по време на тренировка.",
                    Price = 54.90m, 
                    ImageUrl = "/products/FatBurner.png", 
                    Category = "Отслабване", 
                    StockQuantity = 70 
                },
                new Product 
                { 
                    Id = 6, 
                    Name = "L-Carnitine", 
                    Description = "Аминокиселина, която насърчава използването на мазнините като източник на енергия.", 
                    LongDescription = "Л-карнитинът е естествена аминокиселина, която играе ключова роля в транспортирането на мастни киселини до митохондриите, където те се изгарят за енергия. Идеален за прием 30 минути преди кардио тренировка за максимален ефект на изгаряне на мазнини.",
                    Price = 39.99m, 
                    ImageUrl = "/products/L-Carnitine.png", 
                    Category = "Отслабване", 
                    StockQuantity = 80 
                },
                new Product 
                { 
                    Id = 7, 
                    Name = "Detox Formula", 
                    Description = "Комбинация от билки и антиоксиданти за пречистване и метаболитен баланс.", 
                    LongDescription = "Detox Formula помага за изчистването на организма от токсини и задържана вода. С екстракти от глухарче, бял трън и зелен чай, този продукт подобрява храносмилането и подготвя тялото ви за по-ефективно усвояване на хранителните вещества.",
                    Price = 34.99m, 
                    ImageUrl = "/products/DetoxFormula.png", 
                    Category = "Отслабване", 
                    StockQuantity = 45 
                },
                new Product 
                { 
                    Id = 8, 
                    Name = "Glutamine Recovery", 
                    Description = "Подпомага мускулното възстановяване и намалява умората след тренировка.", 
                    LongDescription = "Глутаминът е най-често срещаната аминокиселина в мускулната тъкан. Допълнителният прием на Glutamine Recovery помага за попълване на запасите от гликоген, намалява мускулната болка и подсилва имунната система, която често се компрометира при тежки физически натоварвания.",
                    Price = 46.90m, 
                    ImageUrl = "/products/GlumatineRecovery.png", 
                    Category = "Регенерация", 
                    StockQuantity = 55 
                },
                new Product 
                { 
                    Id = 9, 
                    Name = "Omega-3", 
                    Description = "Рибено масло с високо съдържание на EPA и DHA за стави и сърдечно здраве.", 
                    LongDescription = "Висококачествени Omega-3 мастни киселини, незаменими за здравето на сърцето, мозъка и очите. Те също така притежават силни противовъзпалителни свойства, които помагат за здравето на ставите и намаляват възпалението след тренировка.",
                    Price = 29.99m, 
                    ImageUrl = "/products/Omega-3.png", 
                    Category = "Регенерация", 
                    StockQuantity = 90 
                },
                new Product 
                { 
                    Id = 10, 
                    Name = "Collagen Boost", 
                    Description = "Формула за по-здрави стави, сухожилия и кожа.", 
                    LongDescription = "Collagen Boost осигурява хидролизиран колаген тип I и III, обогатен с витамин C и хиалуронова киселина. Този комплекс укрепва ставите, сухожилията и връзките, като същевременно подобрява еластичността и хидратацията на кожата.",
                    Price = 49.99m, 
                    ImageUrl = "/products/CollagenBoost.png", 
                    Category = "Регенерация", 
                    StockQuantity = 40 
                },
                new Product 
                { 
                    Id = 11, 
                    Name = "Energy Rush", 
                    Description = "Мощна енергийна напитка за експлозивни тренировки и издръжливост.", 
                    LongDescription = "Заредете тренировката си с експлозивна енергия! Energy Rush съчетава бързи въглехидрати, кофеин и таурин, за да ви даде моментален тласък, който да продължи през цялата сесия. Бъдете спрени и фокусирани до последното повторение.",
                    Price = 35.90m, 
                    ImageUrl = "/products/energyRush.png", 
                    Category = "Висока производителност", 
                    StockQuantity = 70 
                },
                new Product 
                { 
                    Id = 12, 
                    Name = "Pump Formula", 
                    Description = "Аргинин и цитрулин за максимален мускулен напомпващ ефект.", 
                    LongDescription = "Почувствайте как мускулите ви се пълнят с кръв с всяко повторение. Pump Formula разширява кръвоносните съдове благодарение на Аргинин и Цитрулин, осигурявайки по-добро снабдяване на мускулите с кислород и хранителни вещества за невероятен обем и васкуларност.",
                    Price = 42.90m, 
                    ImageUrl = "/products/pumpFormula.png", 
                    Category = "Висока производителност", 
                    StockQuantity = 65 
                },
                new Product 
                { 
                    Id = 13, 
                    Name = "Focus Shot", 
                    Description = "Формула за концентрация и енергия без срив.", 
                    LongDescription = "Малка доза, голям ефект. Focus Shot е концентрирана формула с ноотропици и витамини от група B, предназначена да изостри ума ви и да подобри концентрацията, без треперене или енергиен срив след това. Идеален за тренировки или интензивна умствена работа.",
                    Price = 27.90m, 
                    ImageUrl = "/products/FocusShot.png", 
                    Category = "Висока производителност", 
                    StockQuantity = 100 
                },
                new Product 
                { 
                    Id = 14, 
                    Name = "Vitamin B-3", 
                    Description = "Витамини от група B за енергия и имунна подкрепа.", 
                    LongDescription = "Ниацин (Витамин B-3) е от съществено значение за превръщането на храната в енергия. Той поддържа здравето на нервната система, кожата и храносмилателния тракт, като същевременно помага за намаляване на умората и отпадналостта.",
                    Price = 24.99m, 
                    ImageUrl = "/products/VitaminB-3.png", 
                    Category = "Имунитет", 
                    StockQuantity = 85 
                },
                new Product 
                { 
                    Id = 15, 
                    Name = "Multivitamin", 
                    Description = "Комплекс от витамини и минерали за всекидневна защита.", 
                    LongDescription = "Вашата дневна застраховка за здраве. Нашите мултивитамини покриват 100% от препоръчителния дневен прием на ключови микроелементи, подсилвайки имунитета и общото благосъстояние на активния човек.",
                    Price = 39.90m, 
                    ImageUrl = "/products/Multivitamin.png", 
                    Category = "Имунитет", 
                    StockQuantity = 120 
                },
                new Product 
                { 
                    Id = 16, 
                    Name = "Protein Bar Box", 
                    Description = "Протеинови барчета с високо съдържание на белтъчини и фибри.", 
                    LongDescription = "Вкусна и удобна закуска в движение. Всяко барче съдържа 20г висококачествен протеин и е с ниско съдържание на захар. Перфектният начин да задоволите глада за сладко, без да нарушавате диетата си.",
                    Price = 49.90m, 
                    ImageUrl = "/products/ProteinBarBox.png", 
                    Category = "Здравословна закуска", 
                    StockQuantity = 75 
                },
                new Product 
                { 
                    Id = 17, 
                    Name = "Oats", 
                    Description = "Натурални овесени ядки за енергийна закуска.", 
                    LongDescription = "100% натурални пълнозърнести овесени ядки. Източник на бавни въглехидрати и фибри, които осигуряват продължителна енергия и чувство на ситост. Идеални за сутрешна закуска или като добавка към протеиновия шейк.",
                    Price = 19.99m, 
                    ImageUrl = "/products/Oats.png", 
                    Category = "Здравословна закуска", 
                    StockQuantity = 150 
                },
                new Product 
                { 
                    Id = 18, 
                    Name = "Peanut Butter", 
                    Description = "Фъстъчено масло без захар – източник на здравословни мазнини и протеин.", 
                    LongDescription = "Чисто удоволствие на лъжица! Нашето фъстъчено масло е направено от 100% печени фъстъци, без добавена захар, сол или палмово масло. Естествен източник на протеини и здравословни мазнини.",
                    Price = 29.90m, 
                    ImageUrl = "/products/PeanutButter.png", 
                    Category = "Здравословна закуска", 
                    StockQuantity = 110 
                },
                new Product 
                { 
                    Id = 19, 
                    Name = "GymPower Тениска", 
                    Description = "Спортна тениска от дишаща материя, идеална за тренировки.", 
                    LongDescription = "Тренирайте със стил и комфорт. Тази тениска е изработена от високотехнологична дишаща материя, която отвежда потта и ви държи сухи дори по време на най-интензивните сесии. Еластична кройка, която подчертава физиката.",
                    Price = 39.90m, 
                    ImageUrl = "/images/products/PerformanceT-Shirtblack.png", 
                    Category = "Облекло", 
                    StockQuantity = 40 
                },
                new Product 
                { 
                    Id = 20, 
                    Name = "GymPower Суичър", 
                    Description = "Топъл и стилен суичър за спорт и свободно време.", 
                    LongDescription = "Вашият нов любим суичър. Мека ватирана материя, модерен дизайн и удобна качулка. Перфектен за загряване преди тренировка или за релакс у дома след тежък ден.",
                    Price = 69.90m, 
                    ImageUrl = "/products/GymPowerHodie.png", 
                    Category = "Облекло", 
                    StockQuantity = 35 
                },
                new Product 
                { 
                    Id = 21, 
                    Name = "GymPower Ръкавици", 
                    Description = "Удобни фитнес ръкавици за по-добър захват и защита на дланите.", 
                    LongDescription = "Защитете ръцете си от мазоли и подобрете захвата си. Тези ръкавици са с подсилени длани и дишаща горна част за максимален комфорт и здравина при работа с тежести.",
                    Price = 24.90m, 
                    ImageUrl = "/products/gympowergloves.png", 
                    Category = "Облекло", 
                    StockQuantity = 70 
                },
                new Product { Id = 22, Name = "GymPower Ластици", Description = "Комплект тренировъчни ластици с различна сила на съпротивление.", LongDescription = "Тренирайте навсякъде с този комплект ластици. Включва 5 ластика с различно съпротивление, дръжки и колан за глезени. Перфектни за домашни тренировки, рехабилитация или загрявка.", Price = 29.90m, ImageUrl = "/products/gympowerband.png", Category = "Облекло", StockQuantity = 60 },
                new Product { Id = 23, Name = "True Whey Protein", Description = "Висококачествен суроватъчен протеин за растеж и възстановяване.", LongDescription = "True Whey Protein е създаден за истински резултати. С висока биологична стойност и отличен вкус, той е идеалният партньор за всяка фитнес цел.", Price = 59.99m, ImageUrl = "/products/TrueWheyProtein.png", Category = "Изграждане на мускулна маса", StockQuantity = 60 },
                new Product { Id = 24, Name = "Hydro Whey Zero", Description = "Хидролизиран протеин без мазнини и захар за чисти резултати.", LongDescription = "Най-чистата форма на протеин. Хидролизиран за супер бързо усвояване, без лактоза, без глутен и без захар. Перфектен за периоди на изчистване.", Price = 64.99m, ImageUrl = "/products/HydroWheyZero.png", Category = "Изграждане на мускулна маса", StockQuantity = 55 },
                new Product { Id = 25, Name = "Pure IsoWhey", Description = "Изолат с 90% протеин за бързо възстановяване след интензивни тренировки.", LongDescription = "Микрофилтриран суроватъчен изолат с изключителна чистота. Протеин, който достига до мускулите ви за минути. 0г мазнини, 0г въглехидрати.", Price = 69.99m, ImageUrl = "/products/PureIsoWhey.png", Category = "Изграждане на мускулна маса", StockQuantity = 50 },
                new Product { Id = 26, Name = "Micellar Casein", Description = "Бавноусвоим протеин, който поддържа възстановяването по време на сън.", LongDescription = "Нощното възстановяване е ключово. Мицеларният казеин осигурява бавно освобождаване на аминокиселини, предпазвайки мускулите ви през цялата нощ.", Price = 49.99m, ImageUrl = "/products/MiccelarCasein.png", Category = "Регенерация", StockQuantity = 40 },
                new Product { Id = 27, Name = "Creatine Monohydrate Creapure", Description = "Чист креатин монохидрат за сила, обем и издръжливост.", LongDescription = "Златният стандарт при креатина. 100% Creapure® формула за доказано повишаване на физическата сила и производителност при високоинтензивни тренировки.", Price = 29.99m, ImageUrl = "/products/CreatineMonohydrateCreapure.png", Category = "Регенерация", StockQuantity = 80 },
                new Product { Id = 28, Name = "BCAA Instant", Description = "Аминокиселини с бързо усвояване за защита и растеж на мускулите.", LongDescription = "Мигновено разтворими BCAA аминокиселини. Освежаваща напитка, която поддържа мускулите хидратирани и нахранени по време на тренировка.", Price = 34.99m, ImageUrl = "/products/BCAAInstant.png", Category = "Аминокиселини", StockQuantity = 70 },
                new Product { Id = 29, Name = "EAA Mega Stack", Description = "Пълен спектър есенциални аминокиселини за оптимална регенерация.", LongDescription = "Всички 9 есенциални аминокиселини, от които тялото се нуждае за синтеза на протеин. Мощна формула за сериозни атлети.", Price = 39.99m, ImageUrl = "/products/EAAMegaStack.png", Category = "Аминокиселини", StockQuantity = 65 },
                new Product { Id = 30, Name = "Citrulline Malate Pump", Description = "Подобрява кръвообращението и напомпването по време на тренировка.", LongDescription = "Увеличете издръжливостта и намалете мускулната умора с Цитрулин Малат. Той подобрява кръвотока и доставката на хранителни вещества до мускулите.", Price = 27.99m, ImageUrl = "/products/CitrullineMalatePump.png", Category = "Предтренировъчни", StockQuantity = 60 },
                new Product { Id = 31, Name = "Arginine Alpha-KetoGlutarate (AAKG)", Description = "Аргинин за повече сила и издръжливост.", LongDescription = "Високо бионалична форма на Аргинин. Стимулира производството на азотен оксид за по-добро напомпване и васкуларност.", Price = 25.99m, ImageUrl = "/products/ArginineAlpha-KetoGlutarate(AAKG).png", Category = "Аминокиселини", StockQuantity = 50 },
                new Product { Id = 32, Name = "Focus Power Formula", Description = "Формула за ментален фокус и устойчивост по време на тренировка.", LongDescription = "Останете в зоната. Тази формула ви помага да блокирате разсейванията и да се фокусирате изцяло върху всяко движение и повторение.", Price = 33.99m, ImageUrl = "/products/FocusPowerFormula.png", Category = "Предтренировъчни", StockQuantity = 40 },
                new Product { Id = 33, Name = "Pump 3D", Description = "Бустер за по-добър мускулен обем и кръвоснабдяване.", LongDescription = "Триизмерно напомпване. Комбинация от азотни бустери и креатин за обем, плътност и сила.", Price = 36.99m, ImageUrl = "/products/Pump3D.png", Category = "Предтренировъчни", StockQuantity = 55 },
                new Product { Id = 34, Name = "Black Blood NO Booster", Description = "Мощен бустер с кофеин и бета-аланин за експлозивна сила.", LongDescription = "Само за напреднали. Екстремна доза кофеин и стимуланти за най-тежките ви тренировки. Внимание: Висока ефикасност.", Price = 44.99m, ImageUrl = "/products/BlackBloodNoBooster.png", Category = "Предтренировъчни", StockQuantity = 45 },
                new Product { Id = 35, Name = "Nitro Shot Energy", Description = "Предтренировъчна формула за моментален прилив на енергия.", LongDescription = "Течна енергия в удобна опаковка. Изпийте една ампула 15 минути преди тренировка и усетете разликата.", Price = 32.99m, ImageUrl = "/products/NitroShotEnergy.png", Category = "Предтренировъчни", StockQuantity = 65 },
                new Product { Id = 36, Name = "Thor PreWorkout", Description = "Експлозивен бустер за сила, енергия и фокус.", LongDescription = "Легендарният предтренировъчен продукт. Thor ви дава силата на бог за вашите тренировки.", Price = 39.99m, ImageUrl = "/products/ThorPreWorkout.png", Category = "Предтренировъчни", StockQuantity = 70 },
                new Product { Id = 37, Name = "Vitamin C 1000mg", Description = "Поддържа имунната система и възстановяването след натоварване.", LongDescription = "Мощен антиоксидант. Една таблетка дневно покрива нуждите ви от Витамин С, защитавайки клетките от оксидативен стрес.", Price = 14.99m, ImageUrl = "/products/VitaminC1000mg.png", Category = "Имунитет", StockQuantity = 90 },
                new Product { Id = 38, Name = "VitaPink For Women", Description = "Мултивитамини за жени с биотин, цинк и антиоксиданти.", LongDescription = "Специално разработени за жени. Поддържат здравето на косата, кожата и ноктите, както и общия тонус и енергия.", Price = 24.99m, ImageUrl = "/products/VitaPinkForWomen.png", Category = "Витамини и Минерали", StockQuantity = 80 },
                new Product { Id = 39, Name = "Zinc + Magnesium Formula", Description = "Подобрява съня, възстановяването и нивата на тестостерон.", LongDescription = "ZMA формула за атлети. Подобрява качеството на съня и подпомага естественото производство на анаболни хормони.", Price = 19.99m, ImageUrl = "/products/Zinc+MagnesiumFormula.png", Category = "Витамини и Минерали", StockQuantity = 75 },
                new Product { Id = 40, Name = "Multi Complex", Description = "Мултивитаминен комплекс за енергия и здраве.", LongDescription = "Всичко в едно. Витамини, минерали и ензими за цялостно здраве и жизненост.", Price = 22.99m, ImageUrl = "/products/MultiComplex.png", Category = "Витамини и Минерали", StockQuantity = 60 },
                new Product { Id = 41, Name = "Gym Towel Microfiber", Description = "Мека микрофибърна кърпа с лого GymPower – подходяща за фитнес.", LongDescription = "Бързосъхнеща, лека и компактна. Задължителен аксесоар за всяка тренировъчна чанта.", Price = 14.99m, ImageUrl = "/products/GymTowelMicrofiber.png", Category = "Аксесоари", StockQuantity = 100 },
                new Product { Id = 42, Name = "Resistance Band", Description = "Ластици с различно съпротивление за тренировка у дома и във фитнеса.", LongDescription = "Универсален уред за тренировка. Използвайте за загрявка, стречинг или силови упражнения.", Price = 29.99m, ImageUrl = "/products/ResistanceBandOrange.png", Category = "Аксесоари", StockQuantity = 90 },
                new Product { Id = 43, Name = "Training Gloves Pro", Description = "Професионални ръкавици за сигурен захват и защита.", LongDescription = "Ергономичен дизайн и здрави материали. Предпазват ръцете ви и ви дават стабилност при всяко движение.", Price = 24.99m, ImageUrl = "/products/TrainingGlovesPro.png", Category = "Аксесоари", StockQuantity = 75 },
                new Product { Id = 44, Name = "Lifting Straps", Description = "Каишки за по-добър захват при тежки упражнения.", LongDescription = "Премахнете лимита на хвата си. Фокусирайте се върху целевия мускул, не върху предмишниците.", Price = 19.99m, ImageUrl = "/products/LiftingStraps.png", Category = "Аксесоари", StockQuantity = 70 },
                new Product { Id = 45, Name = "Shaker Bottle Elite 700ml", Description = "Шейкър бутилка с вместимост 700 мл и вградено сито.", LongDescription = "100% непропусклив. Модерен дизайн, лесен за почистване, BPA free.", Price = 9.99m, ImageUrl = "/products/ShakerBottleElite700ml.png", Category = "Аксесоари", StockQuantity = 120 },
                new Product { Id = 46, Name = "Protein Bar Deluxe – Chocolate Peanut", Description = "Протеинов бар с вкус на шоколад и фъстъци.", LongDescription = "Декадентски вкус без вина. Шоколад, карамел и фъстъци в един протеинов бар.", Price = 3.99m, ImageUrl = "/products/ProteinBarDeluxe–ChocolatePeanut.jpg", Category = "Храни и Добавки", StockQuantity = 150 },
                new Product { Id = 47, Name = "GymPower Joggers Carbon Grey", Description = "Мъжки спортни панталони с еластична талия и модерен дизайн.", LongDescription = "Удобство и стил от GymPower. Идеални за фитнес залата или за разходка в парка.", Price = 49.99m, ImageUrl = "/products/JoggersCarbonGrey.png", Category = "Мъжко облекло", StockQuantity = 40 },
                new Product { Id = 48, Name = "Performance T-Shirt Black", Description = "Спортна тениска от дишаща материя, идеална за тренировка и ежедневие.", LongDescription = "Класическо черно, модерна технология. Тениска, която работи толкова здраво, колкото и вие.", Price = 34.99m, ImageUrl = "/products/PerformanceTShirtBlack.png", Category = "Мъжко облекло", StockQuantity = 50 },
                new Product { Id = 49, Name = "Training Tank Top White", Description = "Безръкавна фланелка с лого GymPower – за свобода и комфорт.", LongDescription = "Покажете резултатите от труда си. Свободна кройка за максимален обемен обхват на движение.", Price = 29.99m, ImageUrl = "/products/TrainingTankTopWhite.png", Category = "Мъжко облекло", StockQuantity = 50 },
                new Product { Id = 50, Name = "GymPower Hoodie Orange Edition", Description = "Суичър с контрастни оранжеви детайли и лого GymPower.", LongDescription = "Бъдете забележими. Ексклузивно издание в емблематичните цветове на GymPower.", Price = 54.99m, ImageUrl = "/products/HoodieOrangeEdition.png", Category = "Мъжко облекло", StockQuantity = 35 },
                new Product { Id = 51, Name = "Shorts Flex Fit", Description = "Еластични спортни шорти за тренировки и ежедневие.", LongDescription = "Леки и удобни. Вградена технология за отвеждане на потта.", Price = 32.99m, ImageUrl = "/products/ShortsFlexFit.png", Category = "Мъжко облекло", StockQuantity = 50 },
                new Product { Id = 52, Name = "Seamless Leggings Coral", Description = "Безшевни клинове с висока талия за максимален комфорт.", LongDescription = "Втора кожа. Безшевна технология за нула протриване и максимален комфорт.", Price = 49.99m, ImageUrl = "/products/SeamlessLeggingsCoral.png", Category = "Дамско облекло", StockQuantity = 45 },
                new Product { Id = 53, Name = "Sport Bra Power Fit", Description = "Спортен сутиен с отлична поддръжка и стилен дизайн.", LongDescription = "Сигурност и стил. Подходящ за високоинтензивни тренировки.", Price = 34.99m, ImageUrl = "/products/SportBraPowerFit.png", Category = "Дамско облекло", StockQuantity = 60 },
                new Product { Id = 54, Name = "Crop Top Black Edition", Description = "Модерен къс топ с лого GymPower – комфорт и визия.", LongDescription = "Тренди визия за залата. Комбинира се перфектно с клинове с висока талия.", Price = 29.99m, ImageUrl = "/products/CropTopBlackEdition.png", Category = "Дамско облекло", StockQuantity = 55 },
                new Product { Id = 55, Name = "High Waist Shorts Pink", Description = "Къси дамски панталонки с висока талия и мека материя.", LongDescription = "Свеж цвят и удобна кройка. За слънчеви дни и горещи тренировки.", Price = 27.99m, ImageUrl = "/products/HighWaistShortsPink.png", Category = "Дамско облекло", StockQuantity = 45 },
                new Product { Id = 56, Name = "Oversized Hoodie Lilac", Description = "Оувърсайз суичър с модерна визия и комфортна кройка.", LongDescription = "Уют и мода. Страхотен люляков цвят и свободна кройка.", Price = 59.99m, ImageUrl = "/products/OversizedHoodieLilac.png", Category = "Дамско облекло", StockQuantity = 40 }
            );

            // ✅ Seed ProductImages (Example: Using the same main image multiple times for gallery effect)
            var productImages = new System.Collections.Generic.List<ProductImage>();
            int imageId = 1;

            // Generate 3 images for each product (Main + 2 Duplicates/Placeholders)
            for (int i = 1; i <= 56; i++)
            {
                // We need to know the ImageUrl for the product. Since we configured it above, we can assume a pattern or loop separately.
                // However, accessing the data defined above inside OnModelCreating is slightly tricky without duplication.
                // For simplicity, I will re-map the images manually or use a helper, 
                // BUT actually, I can just use a generic placeholder strategy or copy-paste the specific URLs if I want perfection.
                // To save space and time, I will use a lookup mechanism or just loop the known data if I had it in a variable.
                
                // Let's take a smart shortcuts: I'll hardcode a few key products to have DIFFERENT images if possible,
                // otherwise assign the same image 3 times so the gallery functions.
            }
            
            // Re-defining data for loop access
            var products = new[] {
                 new { Id = 1, Url = "/products/wheyprotein.png" },
                 new { Id = 2, Url = "/products/massgainer.png" },
                 new { Id = 3, Url = "/products/BCAAcomplex.png" },
                 new { Id = 4, Url = "/products/gaseinprotein.png" },
                 new { Id = 5, Url = "/products/FatBurner.png" },
                 new { Id = 6, Url = "/products/L-Carnitine.png" },
                 new { Id = 7, Url = "/products/DetoxFormula.png" },
                 new { Id = 8, Url = "/products/GlumatineRecovery.png" },
                 new { Id = 9, Url = "/products/Omega-3.png" },
                 new { Id = 10, Url = "/products/CollagenBoost.png" },
                 new { Id = 11, Url = "/products/energyRush.png" },
                 new { Id = 12, Url = "/products/pumpFormula.png" },
                 new { Id = 13, Url = "/products/FocusShot.png" },
                 new { Id = 14, Url = "/products/VitaminB-3.png" },
                 new { Id = 15, Url = "/products/Multivitamin.png" },
                 new { Id = 16, Url = "/products/ProteinBarBox.png" },
                 new { Id = 17, Url = "/products/Oats.png" },
                 new { Id = 18, Url = "/products/PeanutButter.png" },
                 new { Id = 19, Url = "/images/products/PerformanceT-Shirtblack.png" },
                 new { Id = 20, Url = "/products/GymPowerHodie.png" },
                 new { Id = 21, Url = "/products/gympowergloves.png" },
                 new { Id = 22, Url = "/products/gympowerband.png" },
                 new { Id = 23, Url = "/products/TrueWheyProtein.png" },
                 new { Id = 24, Url = "/products/HydroWheyZero.png" },
                 new { Id = 25, Url = "/products/PureIsoWhey.png" },
                 new { Id = 26, Url = "/products/MiccelarCasein.png" },
                 new { Id = 27, Url = "/products/CreatineMonohydrateCreapure.png" },
                 new { Id = 28, Url = "/products/BCAAInstant.png" },
                 new { Id = 29, Url = "/products/EAAMegaStack.png" },
                 new { Id = 30, Url = "/products/CitrullineMalatePump.png" },
                 new { Id = 31, Url = "/products/ArginineAlpha-KetoGlutarate(AAKG).png" },
                 new { Id = 32, Url = "/products/FocusPowerFormula.png" },
                 new { Id = 33, Url = "/products/Pump3D.png" },
                 new { Id = 34, Url = "/products/BlackBloodNoBooster.png" },
                 new { Id = 35, Url = "/products/NitroShotEnergy.png" },
                 new { Id = 36, Url = "/products/ThorPreWorkout.png" },
                 new { Id = 37, Url = "/products/VitaminC1000mg.png" },
                 new { Id = 38, Url = "/products/VitaPinkForWomen.png" },
                 new { Id = 39, Url = "/products/Zinc+MagnesiumFormula.png" },
                 new { Id = 40, Url = "/products/MultiComplex.png" },
                 new { Id = 41, Url = "/products/GymTowelMicrofiber.png" },
                 new { Id = 42, Url = "/products/ResistanceBandOrange.png" },
                 new { Id = 43, Url = "/products/TrainingGlovesPro.png" },
                 new { Id = 44, Url = "/products/LiftingStraps.png" },
                 new { Id = 45, Url = "/products/ShakerBottleElite700ml.png" },
                 new { Id = 46, Url = "/products/ProteinBarDeluxe–ChocolatePeanut.jpg" },
                 new { Id = 47, Url = "/products/JoggersCarbonGrey.png" },
                 new { Id = 48, Url = "/products/PerformanceTShirtBlack.png" },
                 new { Id = 49, Url = "/products/TrainingTankTopWhite.png" },
                 new { Id = 50, Url = "/products/HoodieOrangeEdition.png" },
                 new { Id = 51, Url = "/products/ShortsFlexFit.png" },
                 new { Id = 52, Url = "/products/SeamlessLeggingsCoral.png" },
                 new { Id = 53, Url = "/products/SportBraPowerFit.png" },
                 new { Id = 54, Url = "/products/CropTopBlackEdition.png" },
                 new { Id = 55, Url = "/products/HighWaistShortsPink.png" },
                 new { Id = 56, Url = "/products/OversizedHoodieLilac.png" }
            };

            foreach (var p in products)
            {
                // Main image (1)
                productImages.Add(new ProductImage { Id = imageId++, ProductId = p.Id, ImageUrl = p.Url });
                // Second image (2) - reusing same for now to show gallery functionality (user instructions)
                productImages.Add(new ProductImage { Id = imageId++, ProductId = p.Id, ImageUrl = p.Url });
                // Third image (3)
                productImages.Add(new ProductImage { Id = imageId++, ProductId = p.Id, ImageUrl = p.Url });
            }

            modelBuilder.Entity<ProductImage>().HasData(productImages);

            // ✅ Seed ProductVariants
            var variants = new List<ProductVariant>();
            int variantId = 1;

            // Helper to add taste variants for supplement products
            void AddTasteVariants(int productId, params (string taste, int stock)[] tastes)
            {
                foreach (var (taste, stock) in tastes)
                {
                    variants.Add(new ProductVariant
                    {
                        Id = variantId++,
                        ProductId = productId,
                        VariantType = "Вкус",
                        VariantValue = taste,
                        PriceAdjustment = 0,
                        StockQuantity = stock,
                        IsAvailable = true
                    });
                }
            }

            // Helper to add color variants for clothing products
            void AddColorVariants(int productId, params (string color, int stock)[] colors)
            {
                foreach (var (color, stock) in colors)
                {
                    variants.Add(new ProductVariant
                    {
                        Id = variantId++,
                        ProductId = productId,
                        VariantType = "Цвят",
                        VariantValue = color,
                        PriceAdjustment = 0,
                        StockQuantity = stock,
                        IsAvailable = true
                    });
                }
            }

            // Supplements with Taste Variants
            AddTasteVariants(1, ("Шоколад", 20), ("Ванилия", 15), ("Ягода", 15)); // Суроватъчен протеин
            AddTasteVariants(2, ("Шоколад", 15), ("Ванилия", 10), ("Банан", 5)); // Mass Gainer
            AddTasteVariants(3, ("Портокал", 20), ("Лимон", 20), ("Ябълка", 20)); // BCAA Complex
            AddTasteVariants(4, ("Шоколад", 15), ("Ванилия", 15), ("Кокос", 10)); // Casein Protein
            AddTasteVariants(8, ("Неутрален", 30), ("Лимон", 25)); // Glutamine Recovery
            AddTasteVariants(11, ("Червени плодове", 25), ("Тропически", 25), ("Енергийна напитка", 20)); // Energy Rush
            AddTasteVariants(12, ("Портокал", 22), ("Грейпфрут", 22), ("Ябълка", 21)); // Pump Formula
            AddTasteVariants(13, ("Енергийна напитка", 35), ("Кола", 35), ("Лимон", 30)); // Focus Shot
            AddTasteVariants(23, ("Шоколад", 25), ("Ванилия", 20), ("Карамел", 15)); // True Whey Protein
            AddTasteVariants(24, ("Шоколад", 20), ("Ванилия", 20), ("Ягода", 15)); // Hydro Whey Zero
            AddTasteVariants(25, ("Ванилия", 20), ("Шоколад", 15), ("Неутрален", 15)); // Pure IsoWhey
            AddTasteVariants(26, ("Шоколад", 15), ("Ванилия", 15), ("Бисквити и крем", 10)); // Micellar Casein
            AddTasteVariants(28, ("Лимон", 25), ("Портокал", 25), ("Диня", 20)); // BCAA Instant
            AddTasteVariants(29, ("Тропически", 22), ("Портокал", 22), ("Лимон", 21)); // EAA Mega Stack
            AddTasteVariants(30, ("Лимон", 20), ("Грейпфрут", 20), ("Неутрален", 20)); // Citrulline Malate Pump
            AddTasteVariants(31, ("Неутрален", 25), ("Лимон", 25)); // Arginine AAKG
            AddTasteVariants(32, ("Боровинка", 15), ("Тропически", 15), ("Лимон", 10)); // Focus Power Formula
            AddTasteVariants(33, ("Портокал", 20), ("Грейпфрут", 18), ("Ябълка", 17)); // Pump 3D
            AddTasteVariants(34, ("Синя малина", 15), ("Червени плодове", 15), ("Енергийна напитка", 15)); // Black Blood NO Booster
            AddTasteVariants(35, ("Енергийна напитка", 22), ("Портокал", 22), ("Манго", 21)); // Nitro Shot Energy
            AddTasteVariants(36, ("Червени плодове", 25), ("Синя малина", 25), ("Портокал", 20)); // Thor PreWorkout
            AddTasteVariants(46, ("Шоколад и фъстъци", 50), ("Карамел", 50), ("Кокос", 50)); // Protein Bar Deluxe

            // Clothing with Color Variants
            AddColorVariants(19, ("Черен", 15), ("Бял", 10), ("Син", 8), ("Сив", 7)); // GymPower Тениска
            AddColorVariants(20, ("Черен", 15), ("Сив", 10), ("Тъмносин", 10)); // GymPower Суичър
            AddColorVariants(21, ("Черен", 25), ("Червен", 25), ("Син", 20)); // GymPower Ръкавици
            AddColorVariants(22, ("Черен", 20), ("Червен", 20), ("Син", 20)); // GymPower Ластици
            AddColorVariants(42, ("Оранжев", 30), ("Черен", 30), ("Син", 30)); // Resistance Band
            AddColorVariants(43, ("Черен", 25), ("Сив", 25), ("Червен", 25)); // Training Gloves Pro
            AddColorVariants(44, ("Черен", 35), ("Оранжев", 35)); // Lifting Straps
            AddColorVariants(45, ("Черен", 40), ("Бял", 40), ("Оранжев", 40)); // Shaker Bottle Elite
            AddColorVariants(47, ("Въглено сив", 15), ("Черен", 15), ("Тъмносин", 10)); // GymPower Joggers
            AddColorVariants(48, ("Черен", 20), ("Бял", 15), ("Сив", 15)); // Performance T-Shirt Black
            AddColorVariants(49, ("Бял", 20), ("Черен", 15), ("Сив", 15)); // Training Tank Top White
            AddColorVariants(50, ("Оранжев", 12), ("Черен", 12), ("Сив", 11)); // GymPower Hoodie Orange Edition
            AddColorVariants(51, ("Черен", 20), ("Тъмносин", 15), ("Сив", 15)); // Shorts Flex Fit
            AddColorVariants(52, ("Корал", 15), ("Черен", 15), ("Лилав", 15)); // Seamless Leggings Coral
            AddColorVariants(53, ("Черен", 20), ("Син", 20), ("Розов", 20)); // Sport Bra Power Fit
            AddColorVariants(54, ("Черен", 20), ("Бял", 18), ("Сив", 17)); // Crop Top Black Edition
            AddColorVariants(55, ("Розов", 15), ("Лилав", 15), ("Син", 15)); // High Waist Shorts Pink
            AddColorVariants(56, ("Люляк", 15), ("Сив", 13), ("Розов", 12)); // Oversized Hoodie Lilac

            modelBuilder.Entity<ProductVariant>().HasData(variants);

            // ✅ Seed ProductGoalMappings (mapping products to goals based on categories)
            var goalMappings = new List<ProductGoalMapping>();
            int mappingId = 1;

            // Helper to create mappings
            void AddMapping(int productId, string goal, string expLevel = "All", int priority = 5, bool isBest = false, bool isRec = true)
            {
                goalMappings.Add(new ProductGoalMapping
                {
                    Id = mappingId++,
                    ProductId = productId,
                    Goal = goal,
                    ExperienceLevel = expLevel,
                    Priority = priority,
                    IsBestChoice = isBest,
                    IsRecommended = isRec
                });
            }

            // Muscle Building Products
            AddMapping(1, "Изграждане на мускулна маса", "Beginner", 9, true, true); // Whey
            AddMapping(1, "Изграждане на мускулна маса", "Intermediate", 7, false, true);
            AddMapping(2, "Изграждане на мускулна маса", "Beginner", 6, false, true); // Mass Gainer
            AddMapping(3, "Изграждане на мускулна маса", "Intermediate", 8, false, true); // BCAA
            AddMapping(3, "Изграждане на мускулна маса", "Advanced", 7, false, true);
            AddMapping(4, "Изграждане на мускулна маса", "Advanced", 6, false, true); // Casein
            AddMapping(23, "Изграждане на мускулна маса", "Intermediate", 8, true, true); // True Whey
            AddMapping(24, "Изграждане на мускулна маса", "Advanced", 9, false, true); // Hydro Whey
            AddMapping(25, "Изграждане на мускулна маса", "Advanced", 10, true, true); // Pure IsoWhey
            AddMapping(27, "Изграждане на мускулна маса", "Beginner", 8, false, true); // Creatine
            AddMapping(27, "Изграждане на мускулна маса", "Intermediate", 9, true, true);
            AddMapping(27, "Изграждане на мускулна маса", "Advanced", 8, false, true);

            // Fat Loss Products
            AddMapping(5, "Отслабване", "All", 9, true, true); // Fat Burner
            AddMapping(6, "Отслабване", "All", 9, true, true); // L-Carnitine
            AddMapping(7, "Отслабване", "Beginner", 6, false, true); // Detox
            AddMapping(1, "Отслабване", "All", 7, false, true); // Protein for lean mass
            AddMapping(3, "Отслабване", "Intermediate", 7, false, true); // BCAA

            // Energy Products
            AddMapping(11, "Енергия", "All", 8, false, true); // Energy Rush
            AddMapping(36, "Енергия", "Intermediate", 10, true, true); // Thor PreWorkout
            AddMapping(34, "Енергия", "Advanced", 9, true, true); // Black Blood
            AddMapping(35, "Енергия", "Beginner", 7, false, true); // Nitro Shot
            AddMapping(3, "Енергия", "All", 6, false, true); // BCAA for endurance
            AddMapping(12, "Енергия", "All", 7, false, true); // Pump Formula
            AddMapping(13, "Енергия", "All", 7, false, true); // Focus Shot

            // Recovery/Regeneration Products
            AddMapping(8, "Регенерация", "All", 9, true, true); // Glutamine
            AddMapping(9, "Регенерация", "All", 8, true, true); // Omega-3
            AddMapping(10, "Регенерация", "All", 8, false, true); // Collagen
            AddMapping(26, "Регенерация", "All", 7, false, true); // Casein
            AddMapping(15, "Регенерация", "Beginner", 7, false, true); // Multivitamin
            AddMapping(29, "Регенерация", "Advanced", 8, false, true); // EAA

            modelBuilder.Entity<ProductGoalMapping>().HasData(goalMappings);

            // ✅ Seed RecommendedStacks
            modelBuilder.Entity<RecommendedStack>().HasData(
                // Muscle Building Stacks
                new RecommendedStack { Id = 1, Name = "Начинаещ мускулен пакет", Goal = "Изграждане на мускулна маса", ExperienceLevel = "Beginner", Budget = "All", ProductIds = "1,27,15", DisplayOrder = 1, IsActive = true },
                new RecommendedStack { Id = 2, Name = "Напреднал мускулен пакет", Goal = "Изграждане на мускулна маса", ExperienceLevel = "Intermediate", Budget = "All", ProductIds = "23,3,27", DisplayOrder = 2, IsActive = true },
                new RecommendedStack { Id = 3, Name = "Експертен мускулен пакет", Goal = "Изграждане на мускулна маса", ExperienceLevel = "Advanced", Budget = "All", ProductIds = "25,29,33", DisplayOrder = 3, IsActive = true },
                
                // Fat Loss Stacks
                new RecommendedStack { Id = 4, Name = "Начинаещ пакет за отслабване", Goal = "Отслабване", ExperienceLevel = "Beginner", Budget = "All", ProductIds = "5,6,1", DisplayOrder = 4, IsActive = true },
                new RecommendedStack { Id = 5, Name = "Напреднал пакет за отслабване", Goal = "Отслабване", ExperienceLevel = "Intermediate", Budget = "All", ProductIds = "5,6,3", DisplayOrder = 5, IsActive = true },
                new RecommendedStack { Id = 6, Name = "Експертен пакет за отслабване", Goal = "Отслабване", ExperienceLevel = "Advanced", Budget = "All", ProductIds = "5,6,3,7", DisplayOrder = 6, IsActive = true },
                
                // Energy Stacks
                new RecommendedStack { Id = 7, Name = "Начинаещ енергиен пакет", Goal = "Енергия", ExperienceLevel = "Beginner", Budget = "All", ProductIds = "35,3,11", DisplayOrder = 7, IsActive = true },
                new RecommendedStack { Id = 8, Name = "Напреднал енергиен пакет", Goal = "Енергия", ExperienceLevel = "Intermediate", Budget = "All", ProductIds = "36,3,12", DisplayOrder = 8, IsActive = true },
                new RecommendedStack { Id = 9, Name = "Експертен енергиен пакет", Goal = "Енергия", ExperienceLevel = "Advanced", Budget = "All", ProductIds = "34,36,3", DisplayOrder = 9, IsActive = true },
                
                // Recovery Stacks
                new RecommendedStack { Id = 10, Name = "Пакет за възстановяване", Goal = "Регенерация", ExperienceLevel = "All", Budget = "All", ProductIds = "8,9,10", DisplayOrder = 10, IsActive = true },
                new RecommendedStack { Id = 11, Name = "Напреднал пакет за регенерация", Goal = "Регенерация", ExperienceLevel = "Advanced", Budget = "All", ProductIds = "8,9,29", DisplayOrder = 11, IsActive = true }
            );
        }
    }
}
