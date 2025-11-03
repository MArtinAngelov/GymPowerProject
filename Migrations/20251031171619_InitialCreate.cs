using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GymPower.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    ImageUrl = table.Column<string>(type: "TEXT", nullable: false),
                    Category = table.Column<string>(type: "TEXT", nullable: false),
                    StockQuantity = table.Column<int>(type: "INTEGER", nullable: false),
                    IsRecommendedForMassGain = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsRecommendedForWeightLoss = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsRecommendedForMaintenance = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false),
                    Role = table.Column<string>(type: "TEXT", nullable: true),
                    FitnessGoal = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    OrderDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Address = table.Column<string>(type: "decimal(18,2)", nullable: true),
                    TotalPrice = table.Column<decimal>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OrderId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProductId = table.Column<int>(type: "INTEGER", nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Category", "Description", "ImageUrl", "IsRecommendedForMaintenance", "IsRecommendedForMassGain", "IsRecommendedForWeightLoss", "Name", "Price", "StockQuantity" },
                values: new object[,]
                {
                    { 1, "Изграждане на мускулна маса", "Класически протеин за покачване на чиста мускулна маса и възстановяване.", "/products/wheyprotein.png", false, false, false, "Суроватъчен протеин", 69.99m, 50 },
                    { 2, "Изграждане на мускулна маса", "Формула с високо съдържание на калории и въглехидрати за бързо покачване на тегло.", "/products/massgainer.png", false, false, false, "Mass Gainer", 84.90m, 30 },
                    { 3, "Изграждане на мускулна маса", "Аминокиселини с разклонена верига за по-добра мускулна регенерация.", "/products/BCAAcomplex.png", false, false, false, "BCAA Complex", 44.90m, 60 },
                    { 4, "Изграждане на мускулна маса", "Бавноусвоим протеин, идеален преди сън за дълготрайно възстановяване.", "/products/gaseinprotein.png", false, false, false, "Casein Protein", 59.90m, 40 },
                    { 5, "Отслабване", "Термогенна формула, която подпомага изгарянето на мазнините и повишава енергията.", "/products/FatBurner.png", false, false, false, "Fat Burner", 54.90m, 70 },
                    { 6, "Отслабване", "Аминокиселина, която насърчава използването на мазнините като източник на енергия.", "/products/L-Carnitine.png", false, false, false, "L-Carnitine", 39.99m, 80 },
                    { 7, "Отслабване", "Комбинация от билки и антиоксиданти за пречистване и метаболитен баланс.", "/products/DetoxFormula.png", false, false, false, "Detox Formula", 34.99m, 45 },
                    { 8, "Регенерация", "Подпомага мускулното възстановяване и намалява умората след тренировка.", "/products/GlumatineRecovery.png", false, false, false, "Glutamine Recovery", 46.90m, 55 },
                    { 9, "Регенерация", "Рибено масло с високо съдържание на EPA и DHA за стави и сърдечно здраве.", "/products/Omega-3.png", false, false, false, "Omega-3", 29.99m, 90 },
                    { 10, "Регенерация", "Формула за по-здрави стави, сухожилия и кожа.", "/products/CollagenBoost.png", false, false, false, "Collagen Boost", 49.99m, 40 },
                    { 11, "Висока производителност", "Мощна енергийна напитка за експлозивни тренировки и издръжливост.", "/products/energyRush.png", false, false, false, "Energy Rush", 35.90m, 70 },
                    { 12, "Висока производителност", "Аргинин и цитрулин за максимален мускулен напомпващ ефект.", "/products/pumpFormula.png", false, false, false, "Pump Formula", 42.90m, 65 },
                    { 13, "Висока производителност", "Формула за концентрация и енергия без срив.", "/products/FocusShot.png", false, false, false, "Focus Shot", 27.90m, 100 },
                    { 14, "Имунитет", "Витамини от група B за енергия и имунна подкрепа.", "/products/VitaminB-3.png", false, false, false, "Vitamin B-3", 24.99m, 85 },
                    { 15, "Имунитет", "Комплекс от витамини и минерали за всекидневна защита.", "/products/Multivitamin.png", false, false, false, "Multivitamin", 39.90m, 120 },
                    { 16, "Здравословна закуска", "Протеинови барчета с високо съдържание на белтъчини и фибри.", "/products/ProteinBarBox.png", false, false, false, "Protein Bar Box", 49.90m, 75 },
                    { 17, "Здравословна закуска", "Натурални овесени ядки за енергийна закуска.", "/products/Oats.png", false, false, false, "Oats", 19.99m, 150 },
                    { 18, "Здравословна закуска", "Фъстъчено масло без захар – източник на здравословни мазнини и протеин.", "/products/PeanutButter.png", false, false, false, "Peanut Butter", 29.90m, 110 },
                    { 19, "Облекло", "Спортна тениска от дишаща материя, идеална за тренировки.", "/products/PerformanceT-Shirtblack.png", false, false, false, "GymPower Тениска", 39.90m, 40 },
                    { 20, "Облекло", "Топъл и стилен суичър за спорт и свободно време.", "/products/GymPowerHodie.png", false, false, false, "GymPower Суичър", 69.90m, 35 },
                    { 21, "Облекло", "Удобни фитнес ръкавици за по-добър захват и защита на дланите.", "/products/gympowergloves.png", false, false, false, "GymPower Ръкавици", 24.90m, 70 },
                    { 22, "Облекло", "Комплект тренировъчни ластици с различна сила на съпротивление.", "/products/gympowerband.png", false, false, false, "GymPower Ластици", 29.90m, 60 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ProductId",
                table: "OrderItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
