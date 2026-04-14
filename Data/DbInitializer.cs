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

            // Force update images for the gallery to have exactly 3 images (1 main + 2 extra)
            if (context.ProductImages.Any(p => p.ImageUrl == "/products/whey_studio_angle.png") || !context.ProductImages.Any(p => p.ImageUrl == "/products/whey_closeup_label.png"))
            {
                if (context.Products.Any())
                {
                    context.ProductImages.RemoveRange(context.ProductImages);
                    context.SaveChanges();
                    
                    var existingProducts = context.Products.ToList();
                    var images = new List<ProductImage>();
                    foreach (var p in existingProducts)
                    {
                        if (p.Category == "Изграждане на мускулна маса" || p.Category == "Висока производителност" || p.Category == "Регенерация")
                        {
                            images.Add(new ProductImage { ProductId = p.Id, ImageUrl = "/products/whey_closeup_label.png" });
                            images.Add(new ProductImage { ProductId = p.Id, ImageUrl = "/products/whey_gym_environment.png" });
                        }
                        else 
                        {
                            images.Add(new ProductImage { ProductId = p.Id, ImageUrl = p.ImageUrl });
                            images.Add(new ProductImage { ProductId = p.Id, ImageUrl = p.ImageUrl });
                        }
                    }
                    context.ProductImages.AddRange(images);
                    context.SaveChanges();
                }
            }

            // Replace Creatine images
            var creatine = context.Products.Include(p => p.Images).FirstOrDefault(p => p.Name.Contains("Creatine"));
            if (creatine != null && creatine.ImageUrl != "/products/creatine_new_1.png")
            {
                creatine.ImageUrl = "/products/creatine_new_1.png";
                if (creatine.Images != null) {
                    context.ProductImages.RemoveRange(creatine.Images);
                }
                context.ProductImages.Add(new ProductImage { ProductId = creatine.Id, ImageUrl = "/products/creatine_new_1.png" });
                context.ProductImages.Add(new ProductImage { ProductId = creatine.Id, ImageUrl = "/products/creatine_new_2.png" });
                context.ProductImages.Add(new ProductImage { ProductId = creatine.Id, ImageUrl = "/products/creatine_new_3.jpg" });
                context.SaveChanges();
            }

            // Replace Multi Complex images
            var multiComplex = context.Products.Include(p => p.Images).FirstOrDefault(p => p.Name == "Multi Complex");
            if (multiComplex != null && multiComplex.ImageUrl != "/products/multicomplex_new_1.png")
            {
                multiComplex.ImageUrl = "/products/multicomplex_new_1.png";
                if (multiComplex.Images != null) {
                    context.ProductImages.RemoveRange(multiComplex.Images);
                }
                context.ProductImages.Add(new ProductImage { ProductId = multiComplex.Id, ImageUrl = "/products/multicomplex_new_1.png" });
                context.ProductImages.Add(new ProductImage { ProductId = multiComplex.Id, ImageUrl = "/products/multicomplex_new_2.png" });
                context.ProductImages.Add(new ProductImage { ProductId = multiComplex.Id, ImageUrl = "/products/multicomplex_new_3.jpg" });
                context.SaveChanges();
            }

            // Replace Peanut Butter images
            var peanutButter = context.Products.Include(p => p.Images).FirstOrDefault(p => p.Name == "Peanut Butter");
            if (peanutButter != null && peanutButter.ImageUrl != "/products/peanut_butter_new_1.png")
            {
                peanutButter.ImageUrl = "/products/peanut_butter_new_1.png";
                if (peanutButter.Images != null) {
                    context.ProductImages.RemoveRange(peanutButter.Images);
                }
                context.ProductImages.Add(new ProductImage { ProductId = peanutButter.Id, ImageUrl = "/products/peanut_butter_new_1.png" });
                context.ProductImages.Add(new ProductImage { ProductId = peanutButter.Id, ImageUrl = "/products/peanut_butter_new_2.jpg" });
                context.ProductImages.Add(new ProductImage { ProductId = peanutButter.Id, ImageUrl = "/products/peanut_butter_new_3.jpg" });
                context.SaveChanges();
            }

            // Replace BCAA Instant images
            var bcaaInstant = context.Products.Include(p => p.Images).FirstOrDefault(p => p.Name == "BCAA Instant");
            if (bcaaInstant != null && (bcaaInstant.Images == null || bcaaInstant.Images.Count < 3))
            {
                bcaaInstant.ImageUrl = "/products/bcaa_instant_new_1.jpg";
                if (bcaaInstant.Images != null) {
                    context.ProductImages.RemoveRange(bcaaInstant.Images);
                }
                context.ProductImages.Add(new ProductImage { ProductId = bcaaInstant.Id, ImageUrl = "/products/bcaa_instant_new_1.jpg" });
                context.ProductImages.Add(new ProductImage { ProductId = bcaaInstant.Id, ImageUrl = "/products/bcaa_instant_new_2.jpg" });
                context.ProductImages.Add(new ProductImage { ProductId = bcaaInstant.Id, ImageUrl = "/products/bcaa_instant_new_3.png" });
                context.SaveChanges();
            }

            // Replace Hydro Whey Zero images
            var hydroWheyZero = context.Products.Include(p => p.Images).FirstOrDefault(p => p.Name == "Hydro Whey Zero");
            if (hydroWheyZero != null && hydroWheyZero.ImageUrl != "/products/hydro_whey_zero_new_1.jpg")
            {
                hydroWheyZero.ImageUrl = "/products/hydro_whey_zero_new_1.jpg";
                if (hydroWheyZero.Images != null) {
                    context.ProductImages.RemoveRange(hydroWheyZero.Images);
                }
                context.ProductImages.Add(new ProductImage { ProductId = hydroWheyZero.Id, ImageUrl = "/products/hydro_whey_zero_new_1.jpg" });
                context.ProductImages.Add(new ProductImage { ProductId = hydroWheyZero.Id, ImageUrl = "/products/hydro_whey_zero_new_2.jpg" });
                context.ProductImages.Add(new ProductImage { ProductId = hydroWheyZero.Id, ImageUrl = "/products/hydro_whey_zero_new_1.jpg" });
                context.SaveChanges();
            }

            // Replace Mass Gainer images
            var massGainer = context.Products.Include(p => p.Images).FirstOrDefault(p => p.Name == "Mass Gainer");
            if (massGainer != null && massGainer.ImageUrl != "/products/mass_gainer_new_1.jpg")
            {
                massGainer.ImageUrl = "/products/mass_gainer_new_1.jpg";
                if (massGainer.Images != null) {
                    context.ProductImages.RemoveRange(massGainer.Images);
                }
                context.ProductImages.Add(new ProductImage { ProductId = massGainer.Id, ImageUrl = "/products/mass_gainer_new_1.jpg" });
                context.ProductImages.Add(new ProductImage { ProductId = massGainer.Id, ImageUrl = "/products/mass_gainer_new_2.png" });
                context.ProductImages.Add(new ProductImage { ProductId = massGainer.Id, ImageUrl = "/products/mass_gainer_new_3.png" });
                context.SaveChanges();
            }

            // Replace Unisex Hoodie GYMPOWER images (Oversized Hoodie Lilac)
            var hoodie = context.Products.Include(p => p.Images).FirstOrDefault(p => p.Name == "Oversized Hoodie Lilac");
            if (hoodie != null && hoodie.ImageUrl != "/products/hoodie_lilac_new_1.png")
            {
                hoodie.ImageUrl = "/products/hoodie_lilac_new_1.png";
                if (hoodie.Images != null) {
                    context.ProductImages.RemoveRange(hoodie.Images);
                }
                context.ProductImages.Add(new ProductImage { ProductId = hoodie.Id, ImageUrl = "/products/hoodie_lilac_new_1.png" });
                context.ProductImages.Add(new ProductImage { ProductId = hoodie.Id, ImageUrl = "/products/hoodie_lilac_new_2.jpg" });
                context.ProductImages.Add(new ProductImage { ProductId = hoodie.Id, ImageUrl = "/products/hoodie_lilac_new_3.jpg" });
                context.SaveChanges();
            }

            // Replace Women's Sports Bra Flex (Sport Bra Power Fit)
            var sportsBra = context.Products.Include(p => p.Images).FirstOrDefault(p => p.Name == "Sport Bra Power Fit");
            if (sportsBra != null && sportsBra.ImageUrl != "/products/sports_bra_flex_new_1.jpg")
            {
                sportsBra.ImageUrl = "/products/sports_bra_flex_new_1.jpg";
                if (sportsBra.Images != null) {
                    context.ProductImages.RemoveRange(sportsBra.Images);
                }
                context.ProductImages.Add(new ProductImage { ProductId = sportsBra.Id, ImageUrl = "/products/sports_bra_flex_new_1.jpg" });
                context.ProductImages.Add(new ProductImage { ProductId = sportsBra.Id, ImageUrl = "/products/sports_bra_flex_new_2.png" });
                context.ProductImages.Add(new ProductImage { ProductId = sportsBra.Id, ImageUrl = "/products/sports_bra_flex_new_3.jpg" });
                context.SaveChanges();
            }

            // Replace Women's Training Shorts images (High Waist Shorts Pink)
            var shortsPink = context.Products.Include(p => p.Images).FirstOrDefault(p => p.Name == "High Waist Shorts Pink");
            if (shortsPink != null && shortsPink.ImageUrl != "/products/shorts_pink_new_1.jpg")
            {
                shortsPink.ImageUrl = "/products/shorts_pink_new_1.jpg";
                if (shortsPink.Images != null) {
                    context.ProductImages.RemoveRange(shortsPink.Images);
                }
                context.ProductImages.Add(new ProductImage { ProductId = shortsPink.Id, ImageUrl = "/products/shorts_pink_new_1.jpg" });
                context.ProductImages.Add(new ProductImage { ProductId = shortsPink.Id, ImageUrl = "/products/shorts_pink_new_2.jpg" });
                context.ProductImages.Add(new ProductImage { ProductId = shortsPink.Id, ImageUrl = "/products/shorts_pink_new_3.jpg" });
                context.SaveChanges();
            }

            // Replace Women's Seamless Leggings images (Seamless Leggings Coral)
            var leggings = context.Products.Include(p => p.Images).FirstOrDefault(p => p.Name == "Seamless Leggings Coral");
            if (leggings != null && leggings.ImageUrl != "/products/leggings_orange_new_1.png")
            {
                leggings.ImageUrl = "/products/leggings_orange_new_1.png";
                if (leggings.Images != null) {
                    context.ProductImages.RemoveRange(leggings.Images);
                }
                context.ProductImages.Add(new ProductImage { ProductId = leggings.Id, ImageUrl = "/products/leggings_orange_new_1.png" });
                context.ProductImages.Add(new ProductImage { ProductId = leggings.Id, ImageUrl = "/products/leggings_orange_new_2.jpg" });
                context.ProductImages.Add(new ProductImage { ProductId = leggings.Id, ImageUrl = "/products/leggings_orange_new_3.jpg" });
                context.SaveChanges();
            }

            // Replace GymPower Joggers Carbon Grey images
            var joggers = context.Products.Include(p => p.Images).FirstOrDefault(p => p.Name == "GymPower Joggers Carbon Grey");
            if (joggers != null && joggers.ImageUrl != "/products/joggers_carbon_grey_new_1.png")
            {
                joggers.ImageUrl = "/products/joggers_carbon_grey_new_1.png";
                if (joggers.Images != null) {
                    context.ProductImages.RemoveRange(joggers.Images);
                }
                context.ProductImages.Add(new ProductImage { ProductId = joggers.Id, ImageUrl = "/products/joggers_carbon_grey_new_1.png" });
                context.ProductImages.Add(new ProductImage { ProductId = joggers.Id, ImageUrl = "/products/joggers_carbon_grey_new_2.jpg" });
                context.ProductImages.Add(new ProductImage { ProductId = joggers.Id, ImageUrl = "/products/joggers_carbon_grey_new_3.jpg" });
                context.SaveChanges();
            }

            // Replace Resistance Band images
            var resistanceBand = context.Products.Include(p => p.Images).FirstOrDefault(p => p.Name == "Resistance Band");
            if (resistanceBand != null && resistanceBand.ImageUrl != "/products/resistance_band_new_1.jpg")
            {
                resistanceBand.ImageUrl = "/products/resistance_band_new_1.jpg";
                if (resistanceBand.Images != null) {
                    context.ProductImages.RemoveRange(resistanceBand.Images);
                }
                context.ProductImages.Add(new ProductImage { ProductId = resistanceBand.Id, ImageUrl = "/products/resistance_band_new_1.jpg" });
                context.ProductImages.Add(new ProductImage { ProductId = resistanceBand.Id, ImageUrl = "/products/resistance_band_new_2.jpg" });
                context.ProductImages.Add(new ProductImage { ProductId = resistanceBand.Id, ImageUrl = "/products/resistance_band_new_3.jpg" });
                context.SaveChanges();
            }

            // Replace Lifting Straps images
            var liftingStraps = context.Products.Include(p => p.Images).FirstOrDefault(p => p.Name == "Lifting Straps");
            if (liftingStraps != null && liftingStraps.ImageUrl != "/products/lifting_straps_new_1.jpg")
            {
                liftingStraps.ImageUrl = "/products/lifting_straps_new_1.jpg";
                if (liftingStraps.Images != null) {
                    context.ProductImages.RemoveRange(liftingStraps.Images);
                }
                context.ProductImages.Add(new ProductImage { ProductId = liftingStraps.Id, ImageUrl = "/products/lifting_straps_new_1.jpg" });
                context.ProductImages.Add(new ProductImage { ProductId = liftingStraps.Id, ImageUrl = "/products/lifting_straps_new_2.jpg" });
                context.ProductImages.Add(new ProductImage { ProductId = liftingStraps.Id, ImageUrl = "/products/lifting_straps_new_3.jpg" });
                context.SaveChanges();
            }

            // Replace Training Gloves Pro images
            var trainingGloves = context.Products.Include(p => p.Images).FirstOrDefault(p => p.Name == "Training Gloves Pro");
            if (trainingGloves != null && trainingGloves.ImageUrl != "/products/training_gloves_new_1.jpg")
            {
                trainingGloves.ImageUrl = "/products/training_gloves_new_1.jpg";
                if (trainingGloves.Images != null) {
                    context.ProductImages.RemoveRange(trainingGloves.Images);
                }
                context.ProductImages.Add(new ProductImage { ProductId = trainingGloves.Id, ImageUrl = "/products/training_gloves_new_1.jpg" });
                context.ProductImages.Add(new ProductImage { ProductId = trainingGloves.Id, ImageUrl = "/products/training_gloves_new_2.jpg" });
                context.ProductImages.Add(new ProductImage { ProductId = trainingGloves.Id, ImageUrl = "/products/training_gloves_new_3.jpg" });
                context.SaveChanges();
            }

            // Replace Shorts Flex Fit images
            var shortsFlexFit = context.Products.Include(p => p.Images).FirstOrDefault(p => p.Name == "Shorts Flex Fit");
            if (shortsFlexFit != null && shortsFlexFit.ImageUrl != "/products/shorts_flex_fit_new_1.jpg")
            {
                shortsFlexFit.ImageUrl = "/products/shorts_flex_fit_new_1.jpg";
                if (shortsFlexFit.Images != null) {
                    context.ProductImages.RemoveRange(shortsFlexFit.Images);
                }
                context.ProductImages.Add(new ProductImage { ProductId = shortsFlexFit.Id, ImageUrl = "/products/shorts_flex_fit_new_1.jpg" });
                context.ProductImages.Add(new ProductImage { ProductId = shortsFlexFit.Id, ImageUrl = "/products/shorts_flex_fit_new_2.png" });
                context.ProductImages.Add(new ProductImage { ProductId = shortsFlexFit.Id, ImageUrl = "/products/shorts_flex_fit_new_3.jpg" });
                context.SaveChanges();
            }

            // Replace Training Tank Top White images
            var tankTopWhite = context.Products.Include(p => p.Images).FirstOrDefault(p => p.Name == "Training Tank Top White");
            if (tankTopWhite != null && tankTopWhite.ImageUrl != "/products/tank_top_white_new_1.jpg")
            {
                tankTopWhite.ImageUrl = "/products/tank_top_white_new_1.jpg";
                if (tankTopWhite.Images != null) {
                    context.ProductImages.RemoveRange(tankTopWhite.Images);
                }
                context.ProductImages.Add(new ProductImage { ProductId = tankTopWhite.Id, ImageUrl = "/products/tank_top_white_new_1.jpg" });
                context.ProductImages.Add(new ProductImage { ProductId = tankTopWhite.Id, ImageUrl = "/products/tank_top_white_new_2.jpg" });
                context.ProductImages.Add(new ProductImage { ProductId = tankTopWhite.Id, ImageUrl = "/products/tank_top_white_new_3.jpg" });
                context.SaveChanges();
            }

            // Replace Performance T-Shirt Black images
            var tshirtBlack = context.Products.Include(p => p.Images).FirstOrDefault(p => p.Name == "Performance T-Shirt Black");
            if (tshirtBlack != null && tshirtBlack.ImageUrl != "/products/tshirt_black_new_1.png")
            {
                tshirtBlack.ImageUrl = "/products/tshirt_black_new_1.png";
                if (tshirtBlack.Images != null) {
                    context.ProductImages.RemoveRange(tshirtBlack.Images);
                }
                context.ProductImages.Add(new ProductImage { ProductId = tshirtBlack.Id, ImageUrl = "/products/tshirt_black_new_1.png" });
                context.ProductImages.Add(new ProductImage { ProductId = tshirtBlack.Id, ImageUrl = "/products/tshirt_black_new_2.png" });
                context.ProductImages.Add(new ProductImage { ProductId = tshirtBlack.Id, ImageUrl = "/products/tshirt_black_new_3.png" });
                context.SaveChanges();
            }

            // Replace Gym Towel Microfiber images
            var towel = context.Products.Include(p => p.Images).FirstOrDefault(p => p.Name == "Gym Towel Microfiber");
            if (towel != null && towel.ImageUrl != "/products/towel_new_1.jpg")
            {
                towel.ImageUrl = "/products/towel_new_1.jpg";
                if (towel.Images != null) {
                    context.ProductImages.RemoveRange(towel.Images);
                }
                context.ProductImages.Add(new ProductImage { ProductId = towel.Id, ImageUrl = "/products/towel_new_1.jpg" });
                context.ProductImages.Add(new ProductImage { ProductId = towel.Id, ImageUrl = "/products/towel_new_2.jpg" });
                context.ProductImages.Add(new ProductImage { ProductId = towel.Id, ImageUrl = "/products/towel_new_3.jpg" });
                context.SaveChanges();
            }

            // Replace Shaker Bottle images
            var shaker = context.Products.Include(p => p.Images).FirstOrDefault(p => p.Name == "Shaker Bottle Elite 700ml");
            if (shaker != null && shaker.ImageUrl != "/products/shaker_new_1.png")
            {
                shaker.ImageUrl = "/products/shaker_new_1.png";
                if (shaker.Images != null) {
                    context.ProductImages.RemoveRange(shaker.Images);
                }
                context.ProductImages.Add(new ProductImage { ProductId = shaker.Id, ImageUrl = "/products/shaker_new_1.png" });
                context.ProductImages.Add(new ProductImage { ProductId = shaker.Id, ImageUrl = "/products/shaker_new_2.jpg" });
                context.ProductImages.Add(new ProductImage { ProductId = shaker.Id, ImageUrl = "/products/shaker_new_3.jpg" });
                context.SaveChanges();
            }

            // Replace Thor PreWorkout images
            var thorPreWorkout = context.Products.Include(p => p.Images).FirstOrDefault(p => p.Name == "Thor PreWorkout");
            if (thorPreWorkout != null && thorPreWorkout.ImageUrl != "/products/thor_preworkout_new_1.jpg")
            {
                thorPreWorkout.ImageUrl = "/products/thor_preworkout_new_1.jpg";
                if (thorPreWorkout.Images != null) {
                    context.ProductImages.RemoveRange(thorPreWorkout.Images);
                }
                context.ProductImages.Add(new ProductImage { ProductId = thorPreWorkout.Id, ImageUrl = "/products/thor_preworkout_new_1.jpg" });
                context.ProductImages.Add(new ProductImage { ProductId = thorPreWorkout.Id, ImageUrl = "/products/thor_preworkout_new_2.jpg" });
                context.ProductImages.Add(new ProductImage { ProductId = thorPreWorkout.Id, ImageUrl = "/products/thor_preworkout_new_3.jpg" });
                context.SaveChanges();
            }

            // Replace Collagen Boost images
            var collagenBoost = context.Products.Include(p => p.Images).FirstOrDefault(p => p.Name == "Collagen Boost");
            if (collagenBoost != null && collagenBoost.ImageUrl != "/products/collagen_boost_new_1.png")
            {
                collagenBoost.ImageUrl = "/products/collagen_boost_new_1.png";
                if (collagenBoost.Images != null) {
                    context.ProductImages.RemoveRange(collagenBoost.Images);
                }
                context.ProductImages.Add(new ProductImage { ProductId = collagenBoost.Id, ImageUrl = "/products/collagen_boost_new_1.png" });
                context.ProductImages.Add(new ProductImage { ProductId = collagenBoost.Id, ImageUrl = "/products/collagen_boost_new_2.png" });
                context.ProductImages.Add(new ProductImage { ProductId = collagenBoost.Id, ImageUrl = "/products/collagen_boost_new_3.png" });
                context.SaveChanges();
            }

            // Replace Oats images
            var oats = context.Products.Include(p => p.Images).FirstOrDefault(p => p.Name == "Oats");
            if (oats != null && oats.ImageUrl != "/products/oats_new_1.jpg")
            {
                oats.ImageUrl = "/products/oats_new_1.jpg";
                if (oats.Images != null) {
                    context.ProductImages.RemoveRange(oats.Images);
                }
                context.ProductImages.Add(new ProductImage { ProductId = oats.Id, ImageUrl = "/products/oats_new_1.jpg" });
                context.ProductImages.Add(new ProductImage { ProductId = oats.Id, ImageUrl = "/products/oats_new_2.jpg" });
                context.ProductImages.Add(new ProductImage { ProductId = oats.Id, ImageUrl = "/products/oats_new_3.png" });
                context.SaveChanges();
            }

            // Replace Pure IsoWhey images
            var pureIsoWhey = context.Products.Include(p => p.Images).FirstOrDefault(p => p.Name == "Pure IsoWhey");
            if (pureIsoWhey != null && pureIsoWhey.ImageUrl != "/products/pure_iso_whey_new_1.png")
            {
                pureIsoWhey.ImageUrl = "/products/pure_iso_whey_new_1.png";
                if (pureIsoWhey.Images != null) {
                    context.ProductImages.RemoveRange(pureIsoWhey.Images);
                }
                context.ProductImages.Add(new ProductImage { ProductId = pureIsoWhey.Id, ImageUrl = "/products/pure_iso_whey_new_1.png" });
                context.ProductImages.Add(new ProductImage { ProductId = pureIsoWhey.Id, ImageUrl = "/products/pure_iso_whey_new_2.png" });
                context.ProductImages.Add(new ProductImage { ProductId = pureIsoWhey.Id, ImageUrl = "/products/pure_iso_whey_new_3.png" });
                context.SaveChanges();
            }

            // Replace Micellar Casein images
            var micellarCasein = context.Products.Include(p => p.Images).FirstOrDefault(p => p.Name == "Micellar Casein");
            if (micellarCasein != null && micellarCasein.ImageUrl != "/products/micellar_casein_new_1.png")
            {
                micellarCasein.ImageUrl = "/products/micellar_casein_new_1.png";
                if (micellarCasein.Images != null) {
                    context.ProductImages.RemoveRange(micellarCasein.Images);
                }
                context.ProductImages.Add(new ProductImage { ProductId = micellarCasein.Id, ImageUrl = "/products/micellar_casein_new_1.png" });
                context.ProductImages.Add(new ProductImage { ProductId = micellarCasein.Id, ImageUrl = "/products/micellar_casein_new_2.png" });
                context.ProductImages.Add(new ProductImage { ProductId = micellarCasein.Id, ImageUrl = "/products/micellar_casein_new_3.png" });
                context.SaveChanges();
            }

            // Replace Vegan Protein Blend images
            var veganBlend = context.Products.Include(p => p.Images).FirstOrDefault(p => p.Name.Contains("Vegan"));
            
            if (veganBlend == null)
            {
                veganBlend = new Product { 
                    Name = "Vegan Protein Blend", 
                    Description = "Растителен протеин за мускулен растеж и възстановяване.", 
                    LongDescription = "Висококачествен веган протеин, съдържащ грахов, конопен и оризов протеин за пълен аминокиселинен профил.", 
                    Price = 55.99m, 
                    ImageUrl = "/products/vegan_blend_1.png", 
                    Category = "Изграждане на мускулна маса", 
                    StockQuantity = 50 
                };
                context.Products.Add(veganBlend);
                context.SaveChanges();
            }

            if (veganBlend != null && (veganBlend.Images == null || veganBlend.Images.Count < 3))
            {
                veganBlend.ImageUrl = "/products/vegan_blend_1.png";
                if (veganBlend.Images != null) {
                    context.ProductImages.RemoveRange(veganBlend.Images);
                }
                context.ProductImages.Add(new ProductImage { ProductId = veganBlend.Id, ImageUrl = "/products/vegan_blend_1.png" });
                context.ProductImages.Add(new ProductImage { ProductId = veganBlend.Id, ImageUrl = "/products/vegan_blend_2.jpg" });
                context.ProductImages.Add(new ProductImage { ProductId = veganBlend.Id, ImageUrl = "/products/vegan_blend_3.jpg" });
                context.SaveChanges();
            }

            if (context.Products.Any()) return; // ✅ Prevent duplicates if data already exists

            // 1. Seed Products
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Суроватъчен протеин", Description = "Класически протеин за покачване на чиста мускулна маса и възстановяване.", LongDescription = "Този висококачествен суроватъчен протеин е идеалният избор за всеки спортуващ. Съдържа оптимален профил на аминокиселини, които подпомагат бързото възстановяване след тренировка и стимулират мускулния растеж. Приемайте една доза (30г) сутрин на гладно или веднага след тренировка за най-добри резултати.", Price = 69.99m, ImageUrl = "/products/wheyprotein.png", Category = "Изграждане на мускулна маса", StockQuantity = 50 },
                new Product { Id = 2, Name = "Mass Gainer", Description = "Формула с високо съдържание на калории и въглехидрати за бързо покачване на тегло.", LongDescription = "Mass Gainer е специално разработен за тези, които трудно покачват килограми („хардгейнъри“). Със своята формула, богата на комплексни въглехидрати и висококачествен протеин, той осигурява необходимите калории за изграждане на масивна мускулатура. Идеален за прием след тренировка или като междинно хранене.", Price = 84.90m, ImageUrl = "/products/massgainer.png", Category = "Изграждане на мускулна маса", StockQuantity = 30 },
                new Product { Id = 3, Name = "BCAA Complex", Description = "Аминокиселини с разклонена верига за по-добра мускулна регенерация.", LongDescription = "BCAA Complex предоставя незаменимите аминокиселини Левцин, Изолевцин и Валин в оптимално съотношение 2:1:1. Те предпазват мускулите от разграждане (катаболизъм) по време на интензивни тренировки и ускоряват възстановяването. Приемайте преди, по време или след тренировка.", Price = 44.90m, ImageUrl = "/products/BCAAcomplex.png", Category = "Изграждане на мускулна маса", StockQuantity = 60 },
                new Product { Id = 4, Name = "Casein Protein", Description = "Бавноусвоим протеин, идеален преди сън за дълготрайно възстановяване.", LongDescription = "Мицеларният казеин е протеин с бавно освобождаване, който подхранва мускулите ви в продължение на до 8 часа. Той е перфектният избор за прием преди лягане, осигурявайки постоянен приток на аминокиселини през нощта, когато тялото се възстановява най-активно.", Price = 59.90m, ImageUrl = "/products/gaseinprotein.png", Category = "Изграждане на мускулна маса", StockQuantity = 40 },
                new Product { Id = 5, Name = "Fat Burner", Description = "Термогенна формула, която подпомага изгарянето на мазнините и повишава енергията.", LongDescription = "Нашата мощна термогенна формула ускорява метаболизма и помага на тялото да използва мастните депа как източник на енергия. Съдържа кофеин, зелен чай и Л-карнитин, които не само топят мазнините, но и повишават фокуса и издръжливостта по време на тренировка.", Price = 54.90m, ImageUrl = "/products/FatBurner.png", Category = "Отслабване", StockQuantity = 70 },
                new Product { Id = 6, Name = "L-Carnitine", Description = "Аминокиселина, която насърчава използването на мазнините като източник на енергия.", LongDescription = "Л-карнитинът е естествена аминокиселина, която играе ключова роля в транспортирането на мастни киселини до митохондриите, където те се изгарят за енергия. Идеален за прием 30 минути преди кардио тренировка за максимален ефект на изгаряне на мазнини.", Price = 39.99m, ImageUrl = "/products/L-Carnitine.png", Category = "Отслабване", StockQuantity = 80 },
                new Product { Id = 7, Name = "Detox Formula", Description = "Комбинация от билки и антиоксиданти за пречистване и метаболитен баланс.", LongDescription = "Detox Formula помага за изчистването на организма от токсини и задържана вода. С екстракти от глухарче, бял трън и зелен чай, този продукт подобрява храносмилането и подготвя тялото ви за по-ефективно усвояване на хранителните вещества.", Price = 34.99m, ImageUrl = "/products/DetoxFormula.png", Category = "Отслабване", StockQuantity = 45 },
                new Product { Id = 8, Name = "Glutamine Recovery", Description = "Подпомага мускулното възстановяване и намалява умората след тренировка.", LongDescription = "Глутаминът е най-често срещаната аминокиселина в мускулната тъкан. Допълнителният прием на Glutamine Recovery помага за попълване на запасите от гликоген, намалява мускулната болка и подсилва имунната система, която често се компрометира при тежки физически натоварвания.", Price = 46.90m, ImageUrl = "/products/GlumatineRecovery.png", Category = "Регенерация", StockQuantity = 55 },
                new Product { Id = 9, Name = "Omega-3", Description = "Рибено масло с високо съдържание на EPA и DHA за стави и сърдечно здраве.", LongDescription = "Висококачествени Omega-3 мастни киселини, незаменими за здравето на сърцето, мозъка и очите. Те също така притежават силни противовъзпалителни свойства, които помагат за здравето на ставите и намаляват възпалението след тренировка.", Price = 29.99m, ImageUrl = "/products/Omega-3.png", Category = "Регенерация", StockQuantity = 90 },
                new Product { Id = 10, Name = "Collagen Boost", Description = "Формула за по-здрави стави, сухожилия и кожа.", LongDescription = "Collagen Boost осигурява хидролизиран колаген тип I и III, обогатен с витамин C и хиалуронова киселина. Този комплекс укрепва ставите, сухожилията и връзките, като същевременно подобрява еластичността и хидратацията на кожата.", Price = 49.99m, ImageUrl = "/products/CollagenBoost.png", Category = "Регенерация", StockQuantity = 40 },
                new Product { Id = 11, Name = "Energy Rush", Description = "Мощна енергийна напитка за експлозивни тренировки и издръжливост.", LongDescription = "Заредете тренировката си с експлозивна енергия! Energy Rush съчетава бързи въглехидрати, кофеин и таурин, за да ви даде моментален тласък, който да продължи през цялата сесия. Бъдете спрени и фокусирани до последното повторение.", Price = 35.90m, ImageUrl = "/products/energyRush.png", Category = "Висока производителност", StockQuantity = 70 },
                new Product { Id = 12, Name = "Pump Formula", Description = "Аргинин и цитрулин за максимален мускулен напомпващ ефект.", LongDescription = "Почувствайте как мускулите ви се пълнят с кръв с всяко повторение. Pump Formula разширява кръвоносните съдове благодарение на Аргинин и Цитрулин, осигурявайки по-добро снабдяване на мускулите с кислород и хранителни вещества за невероятен обем и васкуларност.", Price = 42.90m, ImageUrl = "/products/pumpFormula.png", Category = "Висока производителност", StockQuantity = 65 },
                new Product { Id = 13, Name = "Focus Shot", Description = "Формула за концентрация и енергия без срив.", LongDescription = "Малка доза, голям ефект. Focus Shot е концентрирана формула с ноотропици и витамини от група B, предназначена да изостри ума ви и да подобри концентрацията, без треперене или енергиен срив след това. Идеален за тренировки или интензивна умствена работа.", Price = 27.90m, ImageUrl = "/products/FocusShot.png", Category = "Висока производителност", StockQuantity = 100 },
                new Product { Id = 14, Name = "Vitamin B-3", Description = "Витамини от група B за енергия и имунна подкрепа.", LongDescription = "Ниацин (Витамин B-3) е от съществено значение за превръщането на храната в енергия. Той поддържа здравето на нервната система, кожата и храносмилателния тракт, като същевременно помага за намаляване на умората и отпадналостта.", Price = 24.99m, ImageUrl = "/products/VitaminB-3.png", Category = "Имунитет", StockQuantity = 85 },
                new Product { Id = 15, Name = "Multivitamin", Description = "Комплекс от витамини и минерали за всекидневна защита.", LongDescription = "Вашата дневна застраховка за здраве. Нашите мултивитамини покриват 100% от препоръчителния дневен прием на ключови микроелементи, подсилвайки имунитета и общото благосъстояние на активния човек.", Price = 39.90m, ImageUrl = "/products/Multivitamin.png", Category = "Имунитет", StockQuantity = 120 },
                new Product { Id = 16, Name = "Protein Bar Box", Description = "Протеинови барчета с високо съдържание на белтъчини и фибри.", LongDescription = "Вкусна и удобна закуска в движение. Всяко барче съдържа 20г висококачествен протеин и е с ниско съдържание на захар. Перфектният начин да задоволите глада за сладко, без да нарушавате диетата си.", Price = 49.90m, ImageUrl = "/products/ProteinBarBox.png", Category = "Здравословна закуска", StockQuantity = 75 },
                new Product { Id = 17, Name = "Oats", Description = "Натурални овесени ядки за енергийна закуска.", LongDescription = "100% натурални пълнозърнести овесени ядки. Източник на бавни въглехидрати и фибри, които осигуряват продължителна енергия и чувство на ситост. Идеални за сутрешна закуска или като добавка към протеиновия шейк.", Price = 19.99m, ImageUrl = "/products/Oats.png", Category = "Здравословна закуска", StockQuantity = 150 },
                new Product { Id = 18, Name = "Peanut Butter", Description = "Фъстъчено масло без захар – източник на здравословни мазнини и протеин.", LongDescription = "Чисто удоволствие на лъжица! Нашето фъстъчено масло е направено от 100% печени фъстъци, без добавена захар, сол или палмово масло. Естествен източник на протеини и здравословни мазнини.", Price = 29.90m, ImageUrl = "/products/PeanutButter.png", Category = "Здравословна закуска", StockQuantity = 110 },
                new Product { Id = 19, Name = "GymPower Тениска", Description = "Спортна тениска от дишаща материя, идеална за тренировки.", LongDescription = "Тренирайте със стил и комфорт. Тази тениска е изработена от високотехнологична дишаща материя, която отвежда потта и ви държи сухи дори по време на най-интензивните сесии. Еластична кройка, която подчертава физиката.", Price = 39.90m, ImageUrl = "/images/products/PerformanceT-Shirtblack.png", Category = "Облекло", StockQuantity = 40 },
                new Product { Id = 20, Name = "GymPower Суичър", Description = "Топъл и стилен суичър за спорт и свободно време.", LongDescription = "Вашият нов любим суичър. Мека ватирана материя, модерен дизайн и удобна качулка. Перфектен за загряване преди тренировка или за релакс у дома след тежък ден.", Price = 69.90m, ImageUrl = "/products/GymPowerHodie.png", Category = "Облекло", StockQuantity = 35 },
                new Product { Id = 21, Name = "GymPower Ръкавици", Description = "Удобни фитнес ръкавици за по-добър захват и защита на дланите.", LongDescription = "Защитете ръцете си от мазоли и подобрете захвата си. Тези ръкавици са с подсилени длани и дишаща горна част за максимален комфорт и здравина при работа с тежести.", Price = 24.90m, ImageUrl = "/products/gympowergloves.png", Category = "Облекло", StockQuantity = 70 },
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
                new Product { Id = 52, Name = "Seamless Leggings Coral", Description = "Безшевни клинове with висока талия for maximum comfort.", LongDescription = "Second skin. Seamless technology for zero friction and max comfort.", Price = 49.99m, ImageUrl = "/products/SeamlessLeggingsCoral.png", Category = "Дамско облекло", StockQuantity = 45 },
                new Product { Id = 53, Name = "Sport Bra Power Fit", Description = "Sport bra with excellent support and stylish design.", LongDescription = "Security and style. Suitable for high-intensity training.", Price = 34.99m, ImageUrl = "/products/SportBraPowerFit.png", Category = "Дамско облекло", StockQuantity = 60 },
                new Product { Id = 54, Name = "Crop Top Black Edition", Description = "Modern crop top with GymPower logo – comfort and vision.", LongDescription = "Trendy look for the gym. Combines perfectly with high-waist leggings.", Price = 29.99m, ImageUrl = "/products/CropTopBlackEdition.png", Category = "Дамско облекло", StockQuantity = 55 },
                new Product { Id = 55, Name = "High Waist Shorts Pink", Description = "High-waist shorts and soft fabric.", LongDescription = "Fresh color and comfortable fit. For sunny days and hot workouts.", Price = 27.99m, ImageUrl = "/products/HighWaistShortsPink.png", Category = "Дамско облекло", StockQuantity = 45 },
                new Product { Id = 56, Name = "Oversized Hoodie Lilac", Description = "Oversized hoodie with modern look and comfortable cut.", LongDescription = "Cozy and fashion. Great lilac color and oversized fit.", Price = 59.99m, ImageUrl = "/products/OversizedHoodieLilac.png", Category = "Дамско облекло", StockQuantity = 40 }
            };

            context.Products.AddRange(products);
            context.SaveChanges();

            // 2. Seed ProductImages
            var productImages = new List<ProductImage>();
            foreach (var p in products)
            {
                // For supplements, add the 2 generated variant images as gallery items
                if (p.Category == "Изграждане на мускулна маса" || p.Category == "Висока производителност" || p.Category == "Регенерация")
                {
                    productImages.Add(new ProductImage { ProductId = p.Id, ImageUrl = "/products/whey_closeup_label.png" });
                    productImages.Add(new ProductImage { ProductId = p.Id, ImageUrl = "/products/whey_gym_environment.png" });
                }
                else 
                {
                    // Fallback duplicates if they are not supplements
                    productImages.Add(new ProductImage { ProductId = p.Id, ImageUrl = p.ImageUrl });
                    productImages.Add(new ProductImage { ProductId = p.Id, ImageUrl = p.ImageUrl });
                }
            }
            context.ProductImages.AddRange(productImages);
            context.SaveChanges();

            // 3. Seed ProductVariants
            var variants = new List<ProductVariant>();
            
            // Supplement Tastes
            var supplementIds = new[] { 1, 2, 4, 23, 24, 25, 26 };
            foreach (var id in supplementIds)
            {
                variants.Add(new ProductVariant { ProductId = id, VariantType = "Вкус", VariantValue = "Шоколад", StockQuantity = 20 });
                variants.Add(new ProductVariant { ProductId = id, VariantType = "Вкус", VariantValue = "Ванилия", StockQuantity = 20 });
            }

            // Clothing Colors
            var clothingIds = new[] { 19, 20, 21, 22, 42, 43, 44, 45, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56 };
            foreach (var id in clothingIds)
            {
                variants.Add(new ProductVariant { ProductId = id, VariantType = "Цвят", VariantValue = "Черен", StockQuantity = 15 });
                variants.Add(new ProductVariant { ProductId = id, VariantType = "Цвят", VariantValue = "Сив", StockQuantity = 15 });
            }

            context.ProductVariants.AddRange(variants);
            context.SaveChanges();

            // 4. Seed Goal Mappings
            var mappings = new List<ProductGoalMapping>();
            mappings.Add(new ProductGoalMapping { ProductId = 1, Goal = "Изграждане на мускулна маса", ExperienceLevel = "Beginner", Priority = 9, IsBestChoice = true });
            mappings.Add(new ProductGoalMapping { ProductId = 23, Goal = "Изграждане на мускулна маса", ExperienceLevel = "Intermediate", Priority = 8, IsBestChoice = true });
            mappings.Add(new ProductGoalMapping { ProductId = 5, Goal = "Отслабване", ExperienceLevel = "All", Priority = 9, IsBestChoice = true });
            
            context.ProductGoalMappings.AddRange(mappings);
            context.SaveChanges();

            // 5. Seed Recommended Stacks
            var stacks = new List<RecommendedStack>
            {
                new RecommendedStack { Name = "Начинаещ мускулен пакет", Goal = "Изграждане на мускулна маса", ExperienceLevel = "Beginner", ProductIds = "1,27,15", DisplayOrder = 1 },
                new RecommendedStack { Name = "Напреднал мускулен пакет", Goal = "Изграждане на мускулна маса", ExperienceLevel = "Intermediate", ProductIds = "23,3,27", DisplayOrder = 2 },
                new RecommendedStack { Name = "Пакет за отслабване", Goal = "Отслабване", ExperienceLevel = "All", ProductIds = "5,6,1", DisplayOrder = 3 }
            };
            context.RecommendedStacks.AddRange(stacks);
            context.SaveChanges();
        }
    }
}
