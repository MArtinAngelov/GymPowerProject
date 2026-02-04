using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GymPower.Migrations
{
    /// <inheritdoc />
    public partial class GoalSelectorFeature : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductGoalMappings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProductId = table.Column<int>(type: "INTEGER", nullable: false),
                    Goal = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    ExperienceLevel = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Priority = table.Column<int>(type: "INTEGER", nullable: false),
                    IsBestChoice = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsRecommended = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductGoalMappings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductGoalMappings_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecommendedStacks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Goal = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    ExperienceLevel = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Budget = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    ProductIds = table.Column<string>(type: "TEXT", nullable: false),
                    DisplayOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecommendedStacks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserGoalPreferences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    SessionId = table.Column<string>(type: "TEXT", nullable: true),
                    Goal = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    ExperienceLevel = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Budget = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGoalPreferences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserGoalPreferences_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "ProductGoalMappings",
                columns: new[] { "Id", "ExperienceLevel", "Goal", "IsBestChoice", "IsRecommended", "Priority", "ProductId" },
                values: new object[,]
                {
                    { 1, "Beginner", "Изграждане на мускулна маса", true, true, 9, 1 },
                    { 2, "Intermediate", "Изграждане на мускулна маса", false, true, 7, 1 },
                    { 3, "Beginner", "Изграждане на мускулна маса", false, true, 6, 2 },
                    { 4, "Intermediate", "Изграждане на мускулна маса", false, true, 8, 3 },
                    { 5, "Advanced", "Изграждане на мускулна маса", false, true, 7, 3 },
                    { 6, "Advanced", "Изграждане на мускулна маса", false, true, 6, 4 },
                    { 7, "Intermediate", "Изграждане на мускулна маса", true, true, 8, 23 },
                    { 8, "Advanced", "Изграждане на мускулна маса", false, true, 9, 24 },
                    { 9, "Advanced", "Изграждане на мускулна маса", true, true, 10, 25 },
                    { 10, "Beginner", "Изграждане на мускулна маса", false, true, 8, 27 },
                    { 11, "Intermediate", "Изграждане на мускулна маса", true, true, 9, 27 },
                    { 12, "Advanced", "Изграждане на мускулна маса", false, true, 8, 27 },
                    { 13, "All", "Отслабване", true, true, 9, 5 },
                    { 14, "All", "Отслабване", true, true, 9, 6 },
                    { 15, "Beginner", "Отслабване", false, true, 6, 7 },
                    { 16, "All", "Отслабване", false, true, 7, 1 },
                    { 17, "Intermediate", "Отслабване", false, true, 7, 3 },
                    { 18, "All", "Енергия", false, true, 8, 11 },
                    { 19, "Intermediate", "Енергия", true, true, 10, 36 },
                    { 20, "Advanced", "Енергия", true, true, 9, 34 },
                    { 21, "Beginner", "Енергия", false, true, 7, 35 },
                    { 22, "All", "Енергия", false, true, 6, 3 },
                    { 23, "All", "Енергия", false, true, 7, 12 },
                    { 24, "All", "Енергия", false, true, 7, 13 },
                    { 25, "All", "Регенерация", true, true, 9, 8 },
                    { 26, "All", "Регенерация", true, true, 8, 9 },
                    { 27, "All", "Регенерация", false, true, 8, 10 },
                    { 28, "All", "Регенерация", false, true, 7, 26 },
                    { 29, "Beginner", "Регенерация", false, true, 7, 15 },
                    { 30, "Advanced", "Регенерация", false, true, 8, 29 }
                });

            migrationBuilder.InsertData(
                table: "ProductVariants",
                columns: new[] { "Id", "IsAvailable", "PriceAdjustment", "ProductId", "StockQuantity", "VariantType", "VariantValue" },
                values: new object[,]
                {
                    { 1, true, 0m, 1, 20, "Вкус", "Шоколад" },
                    { 2, true, 0m, 1, 15, "Вкус", "Ванилия" },
                    { 3, true, 0m, 1, 15, "Вкус", "Ягода" },
                    { 4, true, 0m, 2, 15, "Вкус", "Шоколад" },
                    { 5, true, 0m, 2, 10, "Вкус", "Ванилия" },
                    { 6, true, 0m, 2, 5, "Вкус", "Банан" },
                    { 7, true, 0m, 3, 20, "Вкус", "Портокал" },
                    { 8, true, 0m, 3, 20, "Вкус", "Лимон" },
                    { 9, true, 0m, 3, 20, "Вкус", "Ябълка" },
                    { 10, true, 0m, 4, 15, "Вкус", "Шоколад" },
                    { 11, true, 0m, 4, 15, "Вкус", "Ванилия" },
                    { 12, true, 0m, 4, 10, "Вкус", "Кокос" },
                    { 13, true, 0m, 8, 30, "Вкус", "Неутрален" },
                    { 14, true, 0m, 8, 25, "Вкус", "Лимон" },
                    { 15, true, 0m, 11, 25, "Вкус", "Червени плодове" },
                    { 16, true, 0m, 11, 25, "Вкус", "Тропически" },
                    { 17, true, 0m, 11, 20, "Вкус", "Енергийна напитка" },
                    { 18, true, 0m, 12, 22, "Вкус", "Портокал" },
                    { 19, true, 0m, 12, 22, "Вкус", "Грейпфрут" },
                    { 20, true, 0m, 12, 21, "Вкус", "Ябълка" },
                    { 21, true, 0m, 13, 35, "Вкус", "Енергийна напитка" },
                    { 22, true, 0m, 13, 35, "Вкус", "Кола" },
                    { 23, true, 0m, 13, 30, "Вкус", "Лимон" },
                    { 24, true, 0m, 23, 25, "Вкус", "Шоколад" },
                    { 25, true, 0m, 23, 20, "Вкус", "Ванилия" },
                    { 26, true, 0m, 23, 15, "Вкус", "Карамел" },
                    { 27, true, 0m, 24, 20, "Вкус", "Шоколад" },
                    { 28, true, 0m, 24, 20, "Вкус", "Ванилия" },
                    { 29, true, 0m, 24, 15, "Вкус", "Ягода" },
                    { 30, true, 0m, 25, 20, "Вкус", "Ванилия" },
                    { 31, true, 0m, 25, 15, "Вкус", "Шоколад" },
                    { 32, true, 0m, 25, 15, "Вкус", "Неутрален" },
                    { 33, true, 0m, 26, 15, "Вкус", "Шоколад" },
                    { 34, true, 0m, 26, 15, "Вкус", "Ванилия" },
                    { 35, true, 0m, 26, 10, "Вкус", "Бисквити и крем" },
                    { 36, true, 0m, 28, 25, "Вкус", "Лимон" },
                    { 37, true, 0m, 28, 25, "Вкус", "Портокал" },
                    { 38, true, 0m, 28, 20, "Вкус", "Диня" },
                    { 39, true, 0m, 29, 22, "Вкус", "Тропически" },
                    { 40, true, 0m, 29, 22, "Вкус", "Портокал" },
                    { 41, true, 0m, 29, 21, "Вкус", "Лимон" },
                    { 42, true, 0m, 30, 20, "Вкус", "Лимон" },
                    { 43, true, 0m, 30, 20, "Вкус", "Грейпфрут" },
                    { 44, true, 0m, 30, 20, "Вкус", "Неутрален" },
                    { 45, true, 0m, 31, 25, "Вкус", "Неутрален" },
                    { 46, true, 0m, 31, 25, "Вкус", "Лимон" },
                    { 47, true, 0m, 32, 15, "Вкус", "Боровинка" },
                    { 48, true, 0m, 32, 15, "Вкус", "Тропически" },
                    { 49, true, 0m, 32, 10, "Вкус", "Лимон" },
                    { 50, true, 0m, 33, 20, "Вкус", "Портокал" },
                    { 51, true, 0m, 33, 18, "Вкус", "Грейпфрут" },
                    { 52, true, 0m, 33, 17, "Вкус", "Ябълка" },
                    { 53, true, 0m, 34, 15, "Вкус", "Синя малина" },
                    { 54, true, 0m, 34, 15, "Вкус", "Червени плодове" },
                    { 55, true, 0m, 34, 15, "Вкус", "Енергийна напитка" },
                    { 56, true, 0m, 35, 22, "Вкус", "Енергийна напитка" },
                    { 57, true, 0m, 35, 22, "Вкус", "Портокал" },
                    { 58, true, 0m, 35, 21, "Вкус", "Манго" },
                    { 59, true, 0m, 36, 25, "Вкус", "Червени плодове" },
                    { 60, true, 0m, 36, 25, "Вкус", "Синя малина" },
                    { 61, true, 0m, 36, 20, "Вкус", "Портокал" },
                    { 62, true, 0m, 46, 50, "Вкус", "Шоколад и фъстъци" },
                    { 63, true, 0m, 46, 50, "Вкус", "Карамел" },
                    { 64, true, 0m, 46, 50, "Вкус", "Кокос" },
                    { 65, true, 0m, 19, 15, "Цвят", "Черен" },
                    { 66, true, 0m, 19, 10, "Цвят", "Бял" },
                    { 67, true, 0m, 19, 8, "Цвят", "Син" },
                    { 68, true, 0m, 19, 7, "Цвят", "Сив" },
                    { 69, true, 0m, 20, 15, "Цвят", "Черен" },
                    { 70, true, 0m, 20, 10, "Цвят", "Сив" },
                    { 71, true, 0m, 20, 10, "Цвят", "Тъмносин" },
                    { 72, true, 0m, 21, 25, "Цвят", "Черен" },
                    { 73, true, 0m, 21, 25, "Цвят", "Червен" },
                    { 74, true, 0m, 21, 20, "Цвят", "Син" },
                    { 75, true, 0m, 22, 20, "Цвят", "Черен" },
                    { 76, true, 0m, 22, 20, "Цвят", "Червен" },
                    { 77, true, 0m, 22, 20, "Цвят", "Син" },
                    { 78, true, 0m, 42, 30, "Цвят", "Оранжев" },
                    { 79, true, 0m, 42, 30, "Цвят", "Черен" },
                    { 80, true, 0m, 42, 30, "Цвят", "Син" },
                    { 81, true, 0m, 43, 25, "Цвят", "Черен" },
                    { 82, true, 0m, 43, 25, "Цвят", "Сив" },
                    { 83, true, 0m, 43, 25, "Цвят", "Червен" },
                    { 84, true, 0m, 44, 35, "Цвят", "Черен" },
                    { 85, true, 0m, 44, 35, "Цвят", "Оранжев" },
                    { 86, true, 0m, 45, 40, "Цвят", "Черен" },
                    { 87, true, 0m, 45, 40, "Цвят", "Бял" },
                    { 88, true, 0m, 45, 40, "Цвят", "Оранжев" },
                    { 89, true, 0m, 47, 15, "Цвят", "Въглено сив" },
                    { 90, true, 0m, 47, 15, "Цвят", "Черен" },
                    { 91, true, 0m, 47, 10, "Цвят", "Тъмносин" },
                    { 92, true, 0m, 48, 20, "Цвят", "Черен" },
                    { 93, true, 0m, 48, 15, "Цвят", "Бял" },
                    { 94, true, 0m, 48, 15, "Цвят", "Сив" },
                    { 95, true, 0m, 49, 20, "Цвят", "Бял" },
                    { 96, true, 0m, 49, 15, "Цвят", "Черен" },
                    { 97, true, 0m, 49, 15, "Цвят", "Сив" },
                    { 98, true, 0m, 50, 12, "Цвят", "Оранжев" },
                    { 99, true, 0m, 50, 12, "Цвят", "Черен" },
                    { 100, true, 0m, 50, 11, "Цвят", "Сив" },
                    { 101, true, 0m, 51, 20, "Цвят", "Черен" },
                    { 102, true, 0m, 51, 15, "Цвят", "Тъмносин" },
                    { 103, true, 0m, 51, 15, "Цвят", "Сив" },
                    { 104, true, 0m, 52, 15, "Цвят", "Корал" },
                    { 105, true, 0m, 52, 15, "Цвят", "Черен" },
                    { 106, true, 0m, 52, 15, "Цвят", "Лилав" },
                    { 107, true, 0m, 53, 20, "Цвят", "Черен" },
                    { 108, true, 0m, 53, 20, "Цвят", "Син" },
                    { 109, true, 0m, 53, 20, "Цвят", "Розов" },
                    { 110, true, 0m, 54, 20, "Цвят", "Черен" },
                    { 111, true, 0m, 54, 18, "Цвят", "Бял" },
                    { 112, true, 0m, 54, 17, "Цвят", "Сив" },
                    { 113, true, 0m, 55, 15, "Цвят", "Розов" },
                    { 114, true, 0m, 55, 15, "Цвят", "Лилав" },
                    { 115, true, 0m, 55, 15, "Цвят", "Син" },
                    { 116, true, 0m, 56, 15, "Цвят", "Люляк" },
                    { 117, true, 0m, 56, 13, "Цвят", "Сив" },
                    { 118, true, 0m, 56, 12, "Цвят", "Розов" }
                });

            migrationBuilder.InsertData(
                table: "RecommendedStacks",
                columns: new[] { "Id", "Budget", "DisplayOrder", "ExperienceLevel", "Goal", "IsActive", "Name", "ProductIds" },
                values: new object[,]
                {
                    { 1, "All", 1, "Beginner", "Изграждане на мускулна маса", true, "Начинаещ мускулен пакет", "1,27,15" },
                    { 2, "All", 2, "Intermediate", "Изграждане на мускулна маса", true, "Напреднал мускулен пакет", "23,3,27" },
                    { 3, "All", 3, "Advanced", "Изграждане на мускулна маса", true, "Експертен мускулен пакет", "25,29,33" },
                    { 4, "All", 4, "Beginner", "Отслабване", true, "Начинаещ пакет за отслабване", "5,6,1" },
                    { 5, "All", 5, "Intermediate", "Отслабване", true, "Напреднал пакет за отслабване", "5,6,3" },
                    { 6, "All", 6, "Advanced", "Отслабване", true, "Експертен пакет за отслабване", "5,6,3,7" },
                    { 7, "All", 7, "Beginner", "Енергия", true, "Начинаещ енергиен пакет", "35,3,11" },
                    { 8, "All", 8, "Intermediate", "Енергия", true, "Напреднал енергиен пакет", "36,3,12" },
                    { 9, "All", 9, "Advanced", "Енергия", true, "Експертен енергиен пакет", "34,36,3" },
                    { 10, "All", 10, "All", "Регенерация", true, "Пакет за възстановяване", "8,9,10" },
                    { 11, "All", 11, "Advanced", "Регенерация", true, "Напреднал пакет за регенерация", "8,9,29" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductGoalMappings_ProductId",
                table: "ProductGoalMappings",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGoalPreferences_UserId",
                table: "UserGoalPreferences",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductGoalMappings");

            migrationBuilder.DropTable(
                name: "RecommendedStacks");

            migrationBuilder.DropTable(
                name: "UserGoalPreferences");

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 63);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 64);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 65);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 66);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 67);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 68);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 69);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 70);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 71);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 72);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 73);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 74);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 75);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 76);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 77);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 78);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 79);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 80);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 81);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 82);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 83);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 84);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 85);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 86);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 87);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 88);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 89);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 90);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 91);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 92);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 93);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 94);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 95);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 96);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 97);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 98);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 99);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 100);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 103);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 104);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 105);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 106);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 107);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 108);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 109);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 110);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 111);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 112);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 113);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 114);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 115);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 116);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 117);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 118);
        }
    }
}
