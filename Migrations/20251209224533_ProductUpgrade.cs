using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GymPower.Migrations
{
    /// <inheritdoc />
    public partial class ProductUpgrade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FitnessGoal",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LongDescription",
                table: "Products",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Orders",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ProductImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProductId = table.Column<int>(type: "INTEGER", nullable: false),
                    ImageUrl = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductImages_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ProductImages",
                columns: new[] { "Id", "ImageUrl", "ProductId" },
                values: new object[,]
                {
                    { 1, "/products/wheyprotein.png", 1 },
                    { 2, "/products/wheyprotein.png", 1 },
                    { 3, "/products/wheyprotein.png", 1 },
                    { 4, "/products/massgainer.png", 2 },
                    { 5, "/products/massgainer.png", 2 },
                    { 6, "/products/massgainer.png", 2 },
                    { 7, "/products/BCAAcomplex.png", 3 },
                    { 8, "/products/BCAAcomplex.png", 3 },
                    { 9, "/products/BCAAcomplex.png", 3 },
                    { 10, "/products/gaseinprotein.png", 4 },
                    { 11, "/products/gaseinprotein.png", 4 },
                    { 12, "/products/gaseinprotein.png", 4 },
                    { 13, "/products/FatBurner.png", 5 },
                    { 14, "/products/FatBurner.png", 5 },
                    { 15, "/products/FatBurner.png", 5 },
                    { 16, "/products/L-Carnitine.png", 6 },
                    { 17, "/products/L-Carnitine.png", 6 },
                    { 18, "/products/L-Carnitine.png", 6 },
                    { 19, "/products/DetoxFormula.png", 7 },
                    { 20, "/products/DetoxFormula.png", 7 },
                    { 21, "/products/DetoxFormula.png", 7 },
                    { 22, "/products/GlumatineRecovery.png", 8 },
                    { 23, "/products/GlumatineRecovery.png", 8 },
                    { 24, "/products/GlumatineRecovery.png", 8 },
                    { 25, "/products/Omega-3.png", 9 },
                    { 26, "/products/Omega-3.png", 9 },
                    { 27, "/products/Omega-3.png", 9 },
                    { 28, "/products/CollagenBoost.png", 10 },
                    { 29, "/products/CollagenBoost.png", 10 },
                    { 30, "/products/CollagenBoost.png", 10 },
                    { 31, "/products/energyRush.png", 11 },
                    { 32, "/products/energyRush.png", 11 },
                    { 33, "/products/energyRush.png", 11 },
                    { 34, "/products/pumpFormula.png", 12 },
                    { 35, "/products/pumpFormula.png", 12 },
                    { 36, "/products/pumpFormula.png", 12 },
                    { 37, "/products/FocusShot.png", 13 },
                    { 38, "/products/FocusShot.png", 13 },
                    { 39, "/products/FocusShot.png", 13 },
                    { 40, "/products/VitaminB-3.png", 14 },
                    { 41, "/products/VitaminB-3.png", 14 },
                    { 42, "/products/VitaminB-3.png", 14 },
                    { 43, "/products/Multivitamin.png", 15 },
                    { 44, "/products/Multivitamin.png", 15 },
                    { 45, "/products/Multivitamin.png", 15 },
                    { 46, "/products/ProteinBarBox.png", 16 },
                    { 47, "/products/ProteinBarBox.png", 16 },
                    { 48, "/products/ProteinBarBox.png", 16 },
                    { 49, "/products/Oats.png", 17 },
                    { 50, "/products/Oats.png", 17 },
                    { 51, "/products/Oats.png", 17 },
                    { 52, "/products/PeanutButter.png", 18 },
                    { 53, "/products/PeanutButter.png", 18 },
                    { 54, "/products/PeanutButter.png", 18 },
                    { 55, "/images/products/PerformanceT-Shirtblack.png", 19 },
                    { 56, "/images/products/PerformanceT-Shirtblack.png", 19 },
                    { 57, "/images/products/PerformanceT-Shirtblack.png", 19 },
                    { 58, "/products/GymPowerHodie.png", 20 },
                    { 59, "/products/GymPowerHodie.png", 20 },
                    { 60, "/products/GymPowerHodie.png", 20 },
                    { 61, "/products/gympowergloves.png", 21 },
                    { 62, "/products/gympowergloves.png", 21 },
                    { 63, "/products/gympowergloves.png", 21 },
                    { 64, "/products/gympowerband.png", 22 },
                    { 65, "/products/gympowerband.png", 22 },
                    { 66, "/products/gympowerband.png", 22 },
                    { 67, "/products/TrueWheyProtein.png", 23 },
                    { 68, "/products/TrueWheyProtein.png", 23 },
                    { 69, "/products/TrueWheyProtein.png", 23 },
                    { 70, "/products/HydroWheyZero.png", 24 },
                    { 71, "/products/HydroWheyZero.png", 24 },
                    { 72, "/products/HydroWheyZero.png", 24 },
                    { 73, "/products/PureIsoWhey.png", 25 },
                    { 74, "/products/PureIsoWhey.png", 25 },
                    { 75, "/products/PureIsoWhey.png", 25 },
                    { 76, "/products/MiccelarCasein.png", 26 },
                    { 77, "/products/MiccelarCasein.png", 26 },
                    { 78, "/products/MiccelarCasein.png", 26 },
                    { 79, "/products/CreatineMonohydrateCreapure.png", 27 },
                    { 80, "/products/CreatineMonohydrateCreapure.png", 27 },
                    { 81, "/products/CreatineMonohydrateCreapure.png", 27 },
                    { 82, "/products/BCAAInstant.png", 28 },
                    { 83, "/products/BCAAInstant.png", 28 },
                    { 84, "/products/BCAAInstant.png", 28 },
                    { 85, "/products/EAAMegaStack.png", 29 },
                    { 86, "/products/EAAMegaStack.png", 29 },
                    { 87, "/products/EAAMegaStack.png", 29 },
                    { 88, "/products/CitrullineMalatePump.png", 30 },
                    { 89, "/products/CitrullineMalatePump.png", 30 },
                    { 90, "/products/CitrullineMalatePump.png", 30 },
                    { 91, "/products/ArginineAlpha-KetoGlutarate(AAKG).png", 31 },
                    { 92, "/products/ArginineAlpha-KetoGlutarate(AAKG).png", 31 },
                    { 93, "/products/ArginineAlpha-KetoGlutarate(AAKG).png", 31 },
                    { 94, "/products/FocusPowerFormula.png", 32 },
                    { 95, "/products/FocusPowerFormula.png", 32 },
                    { 96, "/products/FocusPowerFormula.png", 32 },
                    { 97, "/products/Pump3D.png", 33 },
                    { 98, "/products/Pump3D.png", 33 },
                    { 99, "/products/Pump3D.png", 33 },
                    { 100, "/products/BlackBloodNoBooster.png", 34 },
                    { 101, "/products/BlackBloodNoBooster.png", 34 },
                    { 102, "/products/BlackBloodNoBooster.png", 34 },
                    { 103, "/products/NitroShotEnergy.png", 35 },
                    { 104, "/products/NitroShotEnergy.png", 35 },
                    { 105, "/products/NitroShotEnergy.png", 35 },
                    { 106, "/products/ThorPreWorkout.png", 36 },
                    { 107, "/products/ThorPreWorkout.png", 36 },
                    { 108, "/products/ThorPreWorkout.png", 36 },
                    { 109, "/products/VitaminC1000mg.png", 37 },
                    { 110, "/products/VitaminC1000mg.png", 37 },
                    { 111, "/products/VitaminC1000mg.png", 37 },
                    { 112, "/products/VitaPinkForWomen.png", 38 },
                    { 113, "/products/VitaPinkForWomen.png", 38 },
                    { 114, "/products/VitaPinkForWomen.png", 38 },
                    { 115, "/products/Zinc+MagnesiumFormula.png", 39 },
                    { 116, "/products/Zinc+MagnesiumFormula.png", 39 },
                    { 117, "/products/Zinc+MagnesiumFormula.png", 39 },
                    { 118, "/products/MultiComplex.png", 40 },
                    { 119, "/products/MultiComplex.png", 40 },
                    { 120, "/products/MultiComplex.png", 40 },
                    { 121, "/products/GymTowelMicrofiber.png", 41 },
                    { 122, "/products/GymTowelMicrofiber.png", 41 },
                    { 123, "/products/GymTowelMicrofiber.png", 41 },
                    { 124, "/products/ResistanceBandOrange.png", 42 },
                    { 125, "/products/ResistanceBandOrange.png", 42 },
                    { 126, "/products/ResistanceBandOrange.png", 42 },
                    { 127, "/products/TrainingGlovesPro.png", 43 },
                    { 128, "/products/TrainingGlovesPro.png", 43 },
                    { 129, "/products/TrainingGlovesPro.png", 43 },
                    { 130, "/products/LiftingStraps.png", 44 },
                    { 131, "/products/LiftingStraps.png", 44 },
                    { 132, "/products/LiftingStraps.png", 44 },
                    { 133, "/products/ShakerBottleElite700ml.png", 45 },
                    { 134, "/products/ShakerBottleElite700ml.png", 45 },
                    { 135, "/products/ShakerBottleElite700ml.png", 45 },
                    { 136, "/products/ProteinBarDeluxe–ChocolatePeanut.jpg", 46 },
                    { 137, "/products/ProteinBarDeluxe–ChocolatePeanut.jpg", 46 },
                    { 138, "/products/ProteinBarDeluxe–ChocolatePeanut.jpg", 46 },
                    { 139, "/products/JoggersCarbonGrey.png", 47 },
                    { 140, "/products/JoggersCarbonGrey.png", 47 },
                    { 141, "/products/JoggersCarbonGrey.png", 47 },
                    { 142, "/products/PerformanceTShirtBlack.png", 48 },
                    { 143, "/products/PerformanceTShirtBlack.png", 48 },
                    { 144, "/products/PerformanceTShirtBlack.png", 48 },
                    { 145, "/products/TrainingTankTopWhite.png", 49 },
                    { 146, "/products/TrainingTankTopWhite.png", 49 },
                    { 147, "/products/TrainingTankTopWhite.png", 49 },
                    { 148, "/products/HoodieOrangeEdition.png", 50 },
                    { 149, "/products/HoodieOrangeEdition.png", 50 },
                    { 150, "/products/HoodieOrangeEdition.png", 50 },
                    { 151, "/products/ShortsFlexFit.png", 51 },
                    { 152, "/products/ShortsFlexFit.png", 51 },
                    { 153, "/products/ShortsFlexFit.png", 51 },
                    { 154, "/products/SeamlessLeggingsCoral.png", 52 },
                    { 155, "/products/SeamlessLeggingsCoral.png", 52 },
                    { 156, "/products/SeamlessLeggingsCoral.png", 52 },
                    { 157, "/products/SportBraPowerFit.png", 53 },
                    { 158, "/products/SportBraPowerFit.png", 53 },
                    { 159, "/products/SportBraPowerFit.png", 53 },
                    { 160, "/products/CropTopBlackEdition.png", 54 },
                    { 161, "/products/CropTopBlackEdition.png", 54 },
                    { 162, "/products/CropTopBlackEdition.png", 54 },
                    { 163, "/products/HighWaistShortsPink.png", 55 },
                    { 164, "/products/HighWaistShortsPink.png", 55 },
                    { 165, "/products/HighWaistShortsPink.png", 55 },
                    { 166, "/products/OversizedHoodieLilac.png", 56 },
                    { 167, "/products/OversizedHoodieLilac.png", 56 },
                    { 168, "/products/OversizedHoodieLilac.png", 56 }
                });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "LongDescription",
                value: "Този висококачествен суроватъчен протеин е идеалният избор за всеки спортуващ. Съдържа оптимален профил на аминокиселини, които подпомагат бързото възстановяване след тренировка и стимулират мускулния растеж. Приемайте една доза (30г) сутрин на гладно или веднага след тренировка за най-добри резултати.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "LongDescription",
                value: "Mass Gainer е специално разработен за тези, които трудно покачват килограми („хардгейнъри“). Със своята формула, богата на комплексни въглехидрати и висококачествен протеин, той осигурява необходимите калории за изграждане на масивна мускулатура. Идеален за прием след тренировка или като междинно хранене.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "LongDescription",
                value: "BCAA Complex предоставя незаменимите аминокиселини Левцин, Изолевцин и Валин в оптимално съотношение 2:1:1. Те предпазват мускулите от разграждане (катаболизъм) по време на интензивни тренировки и ускоряват възстановяването. Приемайте преди, по време или след тренировка.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                column: "LongDescription",
                value: "Мицеларният казеин е протеин с бавно освобождаване, който подхранва мускулите ви в продължение на до 8 часа. Той е перфектният избор за прием преди лягане, осигурявайки постоянен приток на аминокиселини през нощта, когато тялото се възстановява най-активно.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                column: "LongDescription",
                value: "Нашата мощна термогенна формула ускорява метаболизма и помага на тялото да използва мастните депа като източник на енергия. Съдържа кофеин, зелен чай и Л-карнитин, които не само топят мазнините, но и повишават фокуса и издръжливостта по време на тренировка.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                column: "LongDescription",
                value: "Л-карнитинът е естествена аминокиселина, която играе ключова роля в транспортирането на мастни киселини до митохондриите, където те се изгарят за енергия. Идеален за прием 30 минути преди кардио тренировка за максимален ефект на изгаряне на мазнини.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                column: "LongDescription",
                value: "Detox Formula помага за изчистването на организма от токсини и задържана вода. С екстракти от глухарче, бял трън и зелен чай, този продукт подобрява храносмилането и подготвя тялото ви за по-ефективно усвояване на хранителните вещества.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                column: "LongDescription",
                value: "Глутаминът е най-често срещаната аминокиселина в мускулната тъкан. Допълнителният прием на Glutamine Recovery помага за попълване на запасите от гликоген, намалява мускулната болка и подсилва имунната система, която често се компрометира при тежки физически натоварвания.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9,
                column: "LongDescription",
                value: "Висококачествени Omega-3 мастни киселини, незаменими за здравето на сърцето, мозъка и очите. Те също така притежават силни противовъзпалителни свойства, които помагат за здравето на ставите и намаляват възпалението след тренировка.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10,
                column: "LongDescription",
                value: "Collagen Boost осигурява хидролизиран колаген тип I и III, обогатен с витамин C и хиалуронова киселина. Този комплекс укрепва ставите, сухожилията и връзките, като същевременно подобрява еластичността и хидратацията на кожата.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 11,
                column: "LongDescription",
                value: "Заредете тренировката си с експлозивна енергия! Energy Rush съчетава бързи въглехидрати, кофеин и таурин, за да ви даде моментален тласък, който да продължи през цялата сесия. Бъдете спрени и фокусирани до последното повторение.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 12,
                column: "LongDescription",
                value: "Почувствайте как мускулите ви се пълнят с кръв с всяко повторение. Pump Formula разширява кръвоносните съдове благодарение на Аргинин и Цитрулин, осигурявайки по-добро снабдяване на мускулите с кислород и хранителни вещества за невероятен обем и васкуларност.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 13,
                column: "LongDescription",
                value: "Малка доза, голям ефект. Focus Shot е концентрирана формула с ноотропици и витамини от група B, предназначена да изостри ума ви и да подобри концентрацията, без треперене или енергиен срив след това. Идеален за тренировки или интензивна умствена работа.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 14,
                column: "LongDescription",
                value: "Ниацин (Витамин B-3) е от съществено значение за превръщането на храната в енергия. Той поддържа здравето на нервната система, кожата и храносмилателния тракт, като същевременно помага за намаляване на умората и отпадналостта.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 15,
                column: "LongDescription",
                value: "Вашата дневна застраховка за здраве. Нашите мултивитамини покриват 100% от препоръчителния дневен прием на ключови микроелементи, подсилвайки имунитета и общото благосъстояние на активния човек.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 16,
                column: "LongDescription",
                value: "Вкусна и удобна закуска в движение. Всяко барче съдържа 20г висококачествен протеин и е с ниско съдържание на захар. Перфектният начин да задоволите глада за сладко, без да нарушавате диетата си.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 17,
                column: "LongDescription",
                value: "100% натурални пълнозърнести овесени ядки. Източник на бавни въглехидрати и фибри, които осигуряват продължителна енергия и чувство на ситост. Идеални за сутрешна закуска или като добавка към протеиновия шейк.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 18,
                column: "LongDescription",
                value: "Чисто удоволствие на лъжица! Нашето фъстъчено масло е направено от 100% печени фъстъци, без добавена захар, сол или палмово масло. Естествен източник на протеини и здравословни мазнини.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 19,
                column: "LongDescription",
                value: "Тренирайте със стил и комфорт. Тази тениска е изработена от високотехнологична дишаща материя, която отвежда потта и ви държи сухи дори по време на най-интензивните сесии. Еластична кройка, която подчертава физиката.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 20,
                column: "LongDescription",
                value: "Вашият нов любим суичър. Мека ватирана материя, модерен дизайн и удобна качулка. Перфектен за загряване преди тренировка или за релакс у дома след тежък ден.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 21,
                column: "LongDescription",
                value: "Защитете ръцете си от мазоли и подобрете захвата си. Тези ръкавици са с подсилени длани и дишаща горна част за максимален комфорт и здравина при работа с тежести.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 22,
                column: "LongDescription",
                value: "Тренирайте навсякъде с този комплект ластици. Включва 5 ластика с различно съпротивление, дръжки и колан за глезени. Перфектни за домашни тренировки, рехабилитация или загрявка.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 23,
                column: "LongDescription",
                value: "True Whey Protein е създаден за истински резултати. С висока биологична стойност и отличен вкус, той е идеалният партньор за всяка фитнес цел.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 24,
                column: "LongDescription",
                value: "Най-чистата форма на протеин. Хидролизиран за супер бързо усвояване, без лактоза, без глутен и без захар. Перфектен за периоди на изчистване.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 25,
                column: "LongDescription",
                value: "Микрофилтриран суроватъчен изолат с изключителна чистота. Протеин, който достига до мускулите ви за минути. 0г мазнини, 0г въглехидрати.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 26,
                column: "LongDescription",
                value: "Нощното възстановяване е ключово. Мицеларният казеин осигурява бавно освобождаване на аминокиселини, предпазвайки мускулите ви през цялата нощ.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 27,
                column: "LongDescription",
                value: "Златният стандарт при креатина. 100% Creapure® формула за доказано повишаване на физическата сила и производителност при високоинтензивни тренировки.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 28,
                column: "LongDescription",
                value: "Мигновено разтворими BCAA аминокиселини. Освежаваща напитка, която поддържа мускулите хидратирани и нахранени по време на тренировка.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 29,
                column: "LongDescription",
                value: "Всички 9 есенциални аминокиселини, от които тялото се нуждае за синтеза на протеин. Мощна формула за сериозни атлети.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 30,
                column: "LongDescription",
                value: "Увеличете издръжливостта и намалете мускулната умора с Цитрулин Малат. Той подобрява кръвотока и доставката на хранителни вещества до мускулите.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 31,
                column: "LongDescription",
                value: "Високо бионалична форма на Аргинин. Стимулира производството на азотен оксид за по-добро напомпване и васкуларност.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 32,
                column: "LongDescription",
                value: "Останете в зоната. Тази формула ви помага да блокирате разсейванията и да се фокусирате изцяло върху всяко движение и повторение.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 33,
                column: "LongDescription",
                value: "Триизмерно напомпване. Комбинация от азотни бустери и креатин за обем, плътност и сила.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 34,
                column: "LongDescription",
                value: "Само за напреднали. Екстремна доза кофеин и стимуланти за най-тежките ви тренировки. Внимание: Висока ефикасност.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 35,
                column: "LongDescription",
                value: "Течна енергия в удобна опаковка. Изпийте една ампула 15 минути преди тренировка и усетете разликата.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 36,
                column: "LongDescription",
                value: "Легендарният предтренировъчен продукт. Thor ви дава силата на бог за вашите тренировки.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 37,
                column: "LongDescription",
                value: "Мощен антиоксидант. Една таблетка дневно покрива нуждите ви от Витамин С, защитавайки клетките от оксидативен стрес.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 38,
                column: "LongDescription",
                value: "Специално разработени за жени. Поддържат здравето на косата, кожата и ноктите, както и общия тонус и енергия.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 39,
                column: "LongDescription",
                value: "ZMA формула за атлети. Подобрява качеството на съня и подпомага естественото производство на анаболни хормони.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 40,
                column: "LongDescription",
                value: "Всичко в едно. Витамини, минерали и ензими за цялостно здраве и жизненост.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 41,
                column: "LongDescription",
                value: "Бързосъхнеща, лека и компактна. Задължителен аксесоар за всяка тренировъчна чанта.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 42,
                column: "LongDescription",
                value: "Универсален уред за тренировка. Използвайте за загрявка, стречинг или силови упражнения.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 43,
                column: "LongDescription",
                value: "Ергономичен дизайн и здрави материали. Предпазват ръцете ви и ви дават стабилност при всяко движение.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 44,
                column: "LongDescription",
                value: "Премахнете лимита на хвата си. Фокусирайте се върху целевия мускул, не върху предмишниците.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 45,
                column: "LongDescription",
                value: "100% непропусклив. Модерен дизайн, лесен за почистване, BPA free.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 46,
                column: "LongDescription",
                value: "Декадентски вкус без вина. Шоколад, карамел и фъстъци в един протеинов бар.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 47,
                column: "LongDescription",
                value: "Удобство и стил от GymPower. Идеални за фитнес залата или за разходка в парка.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 48,
                column: "LongDescription",
                value: "Класическо черно, модерна технология. Тениска, която работи толкова здраво, колкото и вие.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 49,
                column: "LongDescription",
                value: "Покажете резултатите от труда си. Свободна кройка за максимален обемен обхват на движение.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 50,
                column: "LongDescription",
                value: "Бъдете забележими. Ексклузивно издание в емблематичните цветове на GymPower.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 51,
                column: "LongDescription",
                value: "Леки и удобни. Вградена технология за отвеждане на потта.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 52,
                column: "LongDescription",
                value: "Втора кожа. Безшевна технология за нула протриване и максимален комфорт.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 53,
                column: "LongDescription",
                value: "Сигурност и стил. Подходящ за високоинтензивни тренировки.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 54,
                column: "LongDescription",
                value: "Тренди визия за залата. Комбинира се перфектно с клинове с висока талия.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 55,
                column: "LongDescription",
                value: "Свеж цвят и удобна кройка. За слънчеви дни и горещи тренировки.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 56,
                column: "LongDescription",
                value: "Уют и мода. Страхотен люляков цвят и свободна кройка.");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_ProductId",
                table: "ProductImages",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductImages");

            migrationBuilder.DropColumn(
                name: "LongDescription",
                table: "Products");

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "Users",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "FitnessGoal",
                table: "Users",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Orders",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");
        }
    }
}
