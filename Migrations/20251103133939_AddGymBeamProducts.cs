using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GymPower.Migrations
{
    /// <inheritdoc />
    public partial class AddGymBeamProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Category", "Description", "ImageUrl", "IsRecommendedForMaintenance", "IsRecommendedForMassGain", "IsRecommendedForWeightLoss", "Name", "Price", "StockQuantity" },
                values: new object[,]
                {
                    { 23, "Изграждане на мускулна маса", "Висококачествен суроватъчен протеин за растеж и възстановяване.", "/products/TrueWheyProtein.png", false, false, false, "True Whey Protein", 59.99m, 60 },
                    { 24, "Изграждане на мускулна маса", "Хидролизиран протеин без мазнини и захар за чисти резултати.", "/products/HydroWheyZero.png", false, false, false, "Hydro Whey Zero", 64.99m, 55 },
                    { 25, "Изграждане на мускулна маса", "Изолат с 90% протеин за бързо възстановяване след интензивни тренировки.", "/products/PureIsoWhey.png", false, false, false, "Pure IsoWhey", 69.99m, 50 },
                    { 26, "Регенерация", "Бавноусвоим протеин, който поддържа възстановяването по време на сън.", "/products/MiccelarCasein.png", false, false, false, "Micellar Casein", 49.99m, 40 },
                    { 27, "Регенерация", "Чист креатин монохидрат за сила, обем и издръжливост.", "/products/CreatineMonohydrateCreapure.png", false, false, false, "Creatine Monohydrate Creapure", 29.99m, 80 },
                    { 28, "Аминокиселини", "Аминокиселини с бързо усвояване за защита и растеж на мускулите.", "/products/BCAAInstant.png", false, false, false, "BCAA Instant", 34.99m, 70 },
                    { 29, "Аминокиселини", "Пълен спектър есенциални аминокиселини за оптимална регенерация.", "/products/EAAMegaStack.png", false, false, false, "EAA Mega Stack", 39.99m, 65 },
                    { 30, "Предтренировъчни", "Подобрява кръвообращението и напомпването по време на тренировка.", "/products/CitrullineMalatePump.png", false, false, false, "Citrulline Malate Pump", 27.99m, 60 },
                    { 31, "Аминокиселини", "Аргинин за повече сила и издръжливост.", "/products/ArginineAlpha-KetoGlutarate(AAKG).png", false, false, false, "Arginine Alpha-KetoGlutarate (AAKG)", 25.99m, 50 },
                    { 32, "Предтренировъчни", "Формула за ментален фокус и устойчивост по време на тренировка.", "/products/FocusPowerFormula.png", false, false, false, "Focus Power Formula", 33.99m, 40 },
                    { 33, "Предтренировъчни", "Бустер за по-добър мускулен обем и кръвоснабдяване.", "/products/Pump3D.png", false, false, false, "Pump 3D", 36.99m, 55 },
                    { 34, "Предтренировъчни", "Мощен бустер с кофеин и бета-аланин за експлозивна сила.", "/products/BlackBloodNoBooster.png", false, false, false, "Black Blood NO Booster", 44.99m, 45 },
                    { 35, "Предтренировъчни", "Предтренировъчна формула за моментален прилив на енергия.", "/products/NitroShotEnergy.png", false, false, false, "Nitro Shot Energy", 32.99m, 65 },
                    { 36, "Предтренировъчни", "Експлозивен бустер за сила, енергия и фокус.", "/products/ThorPreWorkout.png", false, false, false, "Thor PreWorkout", 39.99m, 70 },
                    { 37, "Имунитет", "Поддържа имунната система и възстановяването след натоварване.", "/products/VitaminC1000mg.png", false, false, false, "Vitamin C 1000mg", 14.99m, 90 },
                    { 38, "Витамини и Минерали", "Мултивитамини за жени с биотин, цинк и антиоксиданти.", "/products/VitaPinkForWomen.png", false, false, false, "VitaPink For Women", 24.99m, 80 },
                    { 39, "Витамини и Минерали", "Подобрява съня, възстановяването и нивата на тестостерон.", "/products/Zinc+MagnesiumFormula.png", false, false, false, "Zinc + Magnesium Formula", 19.99m, 75 },
                    { 40, "Витамини и Минерали", "Мултивитаминен комплекс за енергия и здраве.", "/products/MultiComplex.png", false, false, false, "Multi Complex", 22.99m, 60 },
                    { 41, "Аксесоари", "Мека микрофибърна кърпа с лого GymPower – подходяща за фитнес.", "/products/GymTowelMicrofiber.png", false, false, false, "Gym Towel Microfiber", 14.99m, 100 },
                    { 42, "Аксесоари", "Ластици с различно съпротивление за тренировка у дома и във фитнеса.", "/products/ResistanceBandOrange.png", false, false, false, "Resistance Band", 29.99m, 90 },
                    { 43, "Аксесоари", "Професионални ръкавици за сигурен захват и защита.", "/products/TrainingGlovesPro.png", false, false, false, "Training Gloves Pro", 24.99m, 75 },
                    { 44, "Аксесоари", "Каишки за по-добър захват при тежки упражнения.", "/products/LiftingStraps.png", false, false, false, "Lifting Straps", 19.99m, 70 },
                    { 45, "Аксесоари", "Шейкър бутилка с вместимост 700 мл и вградено сито.", "/products/ShakerBottleElite700ml.png", false, false, false, "Shaker Bottle Elite 700ml", 9.99m, 120 },
                    { 46, "Храни и Добавки", "Протеинов бар с вкус на шоколад и фъстъци.", "/products/ProteinBarDeluxe–ChocolatePeanut.jpg", false, false, false, "Protein Bar Deluxe – Chocolate Peanut", 3.99m, 150 },
                    { 47, "Мъжко облекло", "Мъжки спортни панталони с еластична талия и модерен дизайн.", "/products/JoggersCarbonGrey.png", false, false, false, "GymPower Joggers Carbon Grey", 49.99m, 40 },
                    { 48, "Мъжко облекло", "Спортна тениска от дишаща материя, идеална за тренировка и ежедневие.", "/products/PerformanceTShirtBlack.png", false, false, false, "Performance T-Shirt Black", 34.99m, 50 },
                    { 49, "Мъжко облекло", "Безръкавна фланелка с лого GymPower – за свобода и комфорт.", "/products/TrainingTankTopWhite.png", false, false, false, "Training Tank Top White", 29.99m, 50 },
                    { 50, "Мъжко облекло", "Суичър с контрастни оранжеви детайли и лого GymPower.", "/products/HoodieOrangeEdition.png", false, false, false, "GymPower Hoodie Orange Edition", 54.99m, 35 },
                    { 51, "Мъжко облекло", "Еластични спортни шорти за тренировки и ежедневие.", "/products/ShortsFlexFit.png", false, false, false, "Shorts Flex Fit", 32.99m, 50 },
                    { 52, "Дамско облекло", "Безшевни клинове с висока талия за максимален комфорт.", "/products/SeamlessLeggingsCoral.png", false, false, false, "Seamless Leggings Coral", 49.99m, 45 },
                    { 53, "Дамско облекло", "Спортен сутиен с отлична поддръжка и стилен дизайн.", "/products/SportBraPowerFit.png", false, false, false, "Sport Bra Power Fit", 34.99m, 60 },
                    { 54, "Дамско облекло", "Модерен къс топ с лого GymPower – комфорт и визия.", "/products/CropTopBlackEdition.png", false, false, false, "Crop Top Black Edition", 29.99m, 55 },
                    { 55, "Дамско облекло", "Къси дамски панталонки с висока талия и мека материя.", "/products/HighWaistShortsPink.png", false, false, false, "High Waist Shorts Pink", 27.99m, 45 },
                    { 56, "Дамско облекло", "Оувърсайз суичър с модерна визия и комфортна кройка.", "/products/OversizedHoodieLilac.png", false, false, false, "Oversized Hoodie Lilac", 59.99m, 40 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 56);
        }
    }
}
