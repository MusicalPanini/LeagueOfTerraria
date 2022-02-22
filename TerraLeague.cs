using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using TerraLeague.UI;
using System.IO;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System.Linq;
using System;
using TerraLeague.Items.SummonerSpells;
using Terraria.GameContent.UI;
using TerraLeague.Projectiles;
using TerraLeague.NPCs;
using Microsoft.Xna.Framework.Graphics;
using TerraLeague.Buffs;
using Terraria.DataStructures;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using TerraLeague.Backgrounds;
using Terraria.GameContent.Shaders;
using TerraLeague.Shaders;
using Terraria.Audio;
using Microsoft.Xna.Framework.Audio;
using TerraLeague.Items.Accessories;
using TerraLeague.NPCs.TargonBoss;
using TerraLeague.Items;
using TerraLeague.Biomes;

namespace TerraLeague
{
    public class TerraLeague : Mod
    {
        internal static string TooltipHeadingColor = "0099cc";
        internal static List<int> DrawHairOverHelmet = new List<int>();

        internal static TerraLeague instance;
        internal int SumCurrencyID;
        
        public static ModKeybind ToggleStats;
        public static ModKeybind Item1;
        public static ModKeybind Item2;
        public static ModKeybind Item3;
        public static ModKeybind Item4;
        public static ModKeybind Item5;
        public static ModKeybind Item6;
        public static ModKeybind Sum1;
        public static ModKeybind Sum2;
        public static ModKeybind QAbility;
        public static ModKeybind WAbility;
        public static ModKeybind EAbility;
        public static ModKeybind RAbility;

        // Config Stuff
        public static bool canLog = false;
        public static bool UseModResourceBar = false;
        public static bool LockUI = false;
        public static bool UseCustomManaRegen = false;
        public static bool UseCustomDefenceStat = false;
        public static bool noItemCooldowns = false;
        public static bool noAbilityCooldowns = false;
        public static bool noSummonerCooldowns = false;
        public static bool unlimitedMana = false;
        public static bool debugMode = false;
        public static float fogIntensity;
        //public static ModKeybind Trinket;

        //public static PlayerLayer ShieldEffect;


        public TerraLeague()
        {
            instance = this;
        }

        /// <summary>
        /// Runs when initally loading the mod
        /// </summary>
        public override void Load()
        {
            Logger.InfoFormat("{0} logging", Name);

            ToggleStats = KeybindLoader.RegisterKeybind(this,"Toggle Stats Page", "L");
            Item1 = KeybindLoader.RegisterKeybind(this, "Active item 1", "D1");
            Item2 = KeybindLoader.RegisterKeybind(this, "Active item 2", "D2");
            Item3 = KeybindLoader.RegisterKeybind(this, "Active item 3", "D3");
            Item4 = KeybindLoader.RegisterKeybind(this, "Active item 4", "D4");
            Item5 = KeybindLoader.RegisterKeybind(this, "Active item 5", "D5");
            Item6 = KeybindLoader.RegisterKeybind(this, "Active item 6", "D6");
            Sum1 = KeybindLoader.RegisterKeybind(this, "Summoner Spell 1", "F");
            Sum2 = KeybindLoader.RegisterKeybind(this, "Summoner Spell 2", "G");
            QAbility = KeybindLoader.RegisterKeybind(this, "Ability 1", "Z");
            WAbility = KeybindLoader.RegisterKeybind(this, "Ability 2", "X");
            EAbility = KeybindLoader.RegisterKeybind(this, "Ability 3", "C");
            RAbility = KeybindLoader.RegisterKeybind(this, "Ability 4", "V");
            //Trinket = RegisterHotKey("Trinket", "R");
            SumCurrencyID = CustomCurrencyManager.RegisterCurrency(new SummonerCurrency(ModContent.ItemType<VialofTrueMagic>(), 999L));

            if (!Main.dedServ)
            {
                //AddEquipTexture(new Items.Accessories.DarkinHead(), null, EquipType.Head, "TerraLeague/Items/Accessories/Darkin_Head");
                //AddEquipTexture(new Items.Accessories.DarkinBody(), null, EquipType.Body, "TerraLeague/Items/Accessories/Darkin_Body", "TerraLeague/Items/Accessories/Darkin_Arms");
                //AddEquipTexture(new Items.Accessories.DarkinLegs(), null, EquipType.Legs, "TerraLeague/Items/Accessories/Darkin_Legs");

                Filters.Scene["TerraLeague:TheBlackMist"] = new Filter(new BlackMistShaderData("FilterSandstormForeground").UseColor(0, 2, 1).UseSecondaryColor(0, 0, 0).UseImage(((Texture2D)Assets.Request<Texture2D>("Textures/Backgrounds/Fog")), 0, null).UseIntensity(1f).UseOpacity(0.2f).UseImageScale(new Vector2(8, 8)), EffectPriority.High);
                Overlays.Scene["TerraLeague:TheBlackMist"] = new SimpleOverlay("Images/Misc/Perlin", new BlackMistShaderData("FilterSandstormBackground").UseColor(0, 1, 0).UseSecondaryColor(0, 0, 0).UseImage((Texture2D)Assets.Request<Texture2D>("Textures/Backgrounds/Fog"), 0, null).UseIntensity(5).UseOpacity(1f).UseImageScale(new Vector2(4, 4)), EffectPriority.High, RenderLayers.Landscape);
                SkyManager.Instance["TerraLeague:TheBlackMist"] = new BlackMistSky();

                Filters.Scene["TerraLeague:Targon"] = new Filter(new TargonShaderData("FilterMiniTower").UseColor(0.0f, 0.3f, 0.8f).UseOpacity(0.7f), EffectPriority.VeryHigh);
                SkyManager.Instance["TerraLeague:Targon"] = new TargonPeakSky();
            }

            Main.instance.GUIBarsDraw();
            base.Load();
        }

        public override void PostSetupContent()
        {
            //Mod bossChecklist = ModLoader.GetMod("BossChecklist");
            //if (bossChecklist != null)
            //{
            //    bossChecklist.Call(
            //        "AddBoss",  // Call
            //        3.1f,       // Boss Progresion
            //        new List<int>() { ModContent.NPCType<TargonBossNPC>() }, // NPC Types
            //        this, // Mod
            //        "The Celestial Gate Keeper", // Name
            //        (Func<bool>)(() => Common.ModSystems.WorldSystem.TargonArenaDefeated), // Completion Check
            //        0, // Spawn Item 
            //        new List<int>() { ModContent.ItemType<Items.Placeable.TargonBossTrophy>() }, // Collection Items
            //        new List<int>() { ModContent.ItemType<Items.CelestialBar>(), ModContent.ItemType<Items.Placeable.TargonMonolith>(), ModContent.ItemType<Items.Accessories.BottleOfStardust>() }, // Drops
            //        "Climb Mount Targon and accept the challenge at its peak", // Spawn Info
            //        "",
            //        "TerraLeague/NPCs/TargonBoss/TargonBoss_Checklist",
            //        "TerraLeague/NPCs/TargonBoss/TargonBoss_Head");


            //    bossChecklist.Call(
            //        "AddEvent",  // Call
            //        2.8f,       // Boss Progresion
            //        new List<int>() { ModContent.NPCType<TheUndying_1>(), ModContent.NPCType<TheUndying_Archer>(), ModContent.NPCType<TheUndying_Necromancer>(), ModContent.NPCType<BansheeHive>(), ModContent.NPCType<EtherealRemitter>(), ModContent.NPCType<FallenCrimera>(), ModContent.NPCType<MistEater>(), ModContent.NPCType<SoulBoundSlime>(), ModContent.NPCType<SpectralBitter>(), ModContent.NPCType<UnleashedSpirit>(), ModContent.NPCType<Scuttlegeist>(), ModContent.NPCType<MistDevourer_Head>(), ModContent.NPCType<ShelledHorror>(), ModContent.NPCType<SpectralShark>(), ModContent.NPCType<Mistwraith>(), ModContent.NPCType<ShadowArtilery>() }, // NPC Types
            //        this, // Mod
            //        "The Harrowing", // Name
            //        (Func<bool>)(() => Common.ModSystems.WorldSystem.BlackMistDefeated), // Completion Check
            //        0, // Spawn Item 
            //        0, // Collection Items
            //        new List<int>() { ModContent.ItemType<EternalFlame>(), ModContent.ItemType<Items.Tools.FadingMemories>(), ModContent.ItemType<Nightbloom>(), ModContent.ItemType<Items.Armor.NecromancersHood>(), ModContent.ItemType<Items.Armor.NecromancersRobe>(), ModContent.ItemType<Items.DamnedSoul>() }, // Drops
            //        "If there is a player with more than 200 max life, there is a 1/12 chance each night for The Harrowing to begin. During New Moons, there is a 1/4 chance instead", // Spawn Info
            //        "",
            //        "TerraLeague/NPCs/BlackMist_Checklist",
            //        "TerraLeague/Gores/MistPuff_1"
            //        );
            //}
        }

        /// <summary>
        /// <para>Runs when disabling the mod.</para>
        /// Be sure to unload any static variables in the mod instance as they are not unloaded automaticly and will cause crashes
        /// </summary>
        public override void Unload()
        {
            instance = null;
            ToggleStats = null;
            Item1 = null;
            Item2 = null;
            Item3 = null;
            Item4 = null;
            Item5 = null;
            Item6 = null;
            Sum1 = null;
            Sum2 = null;
            QAbility = null;
            WAbility = null;
            EAbility = null;
            RAbility = null;
            //Trinket = null;

            base.Unload();
        }


        // Move to Biome class
        //public override void UpdateMusic(ref int music, ref MusicPriority priority)
        //{
        //    if (Main.myPlayer == -1 || Main.gameMenu || !Main.LocalPlayer.active)
        //    {
        //        return;
        //    }
        //    if (Main.LocalPlayer.HasBuff(ModContent.BuffType<InTargonArena>()) && NPC.CountNPCS(ModContent.NPCType<TargonBossNPC>()) > 0)
        //    {
        //        music = MusicID.Boss2;
        //        priority = MusicPriority.BossHigh;
        //    }
        //    if (Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().zoneBlackMist)
        //    {
        //        music = MusicID.Eerie;
        //        priority = MusicPriority.Environment;
        //    }
        //    if (Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().zoneVoidPortal)
        //    {
        //        music = MusicID.Hell;
        //        priority = MusicPriority.Environment;
        //    }
        //    base.UpdateMusic(ref music, ref priority);
        //}

        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            ModNetHandler.HandlePacket(reader, whoAmI);
        }

        public override void AddRecipeGroups()
        {
            #region Pre Hardmode Bars
            RecipeGroup CopperGroup = new RecipeGroup(() => "Copper or Tin Bar", new int[]
            {
                ItemID.CopperBar,
                ItemID.TinBar
            });
            RecipeGroup.RegisterGroup("TerraLeague:CopperGroup", CopperGroup);

            RecipeGroup IronGroup = new RecipeGroup(() => "Iron or Lead Bar", new int[]
            {
                ItemID.IronBar,
                ItemID.LeadBar
            });
            RecipeGroup.RegisterGroup("TerraLeague:IronGroup", IronGroup);

            RecipeGroup SilverGroup = new RecipeGroup(() => "Silver or Tungston Bar", new int[]
            {
                ItemID.SilverBar,
                ItemID.TungstenBar
            });
            RecipeGroup.RegisterGroup("TerraLeague:SilverGroup", SilverGroup);

            RecipeGroup GoldGroup = new RecipeGroup(() => "Gold or Platinum Bar", new int[]
            {
                ItemID.GoldBar,
                ItemID.PlatinumBar
            });
            RecipeGroup.RegisterGroup("TerraLeague:GoldGroup", GoldGroup);

            RecipeGroup DemonGroup = new RecipeGroup(() => "Demonite or Crimtane Bar", new int[]
            {
                ItemID.DemoniteBar,
                ItemID.CrimtaneBar
            });
            RecipeGroup.RegisterGroup("TerraLeague:DemonGroup", DemonGroup);

            RecipeGroup DemonPartGroup = new RecipeGroup(() => "Shadow Scale or Tissue Sample", new int[]
            {
                ItemID.ShadowScale,
                ItemID.TissueSample
            });
            RecipeGroup.RegisterGroup("TerraLeague:DemonPartGroup", DemonPartGroup);

            RecipeGroup EvilDropGroup = new RecipeGroup(() => "Rotten Chunk or Vertebrae", new int[]
            {
                ItemID.RottenChunk,
                ItemID.Vertebrae
            });
            RecipeGroup.RegisterGroup("TerraLeague:EvilDropGroup", EvilDropGroup);
            #endregion

            #region Hardmode Bars
            RecipeGroup Tier1Bar = new RecipeGroup(() => "Cobalt or Palladium Bar", new int[]
            {
                ItemID.CobaltBar,
                ItemID.PalladiumBar
            });
            RecipeGroup.RegisterGroup("TerraLeague:Tier1Bar", Tier1Bar);

            RecipeGroup Tier2Bar = new RecipeGroup(() => "Mythril or Orichalcum Bar", new int[]
            {
                ItemID.MythrilBar,
                ItemID.OrichalcumBar
            });
            RecipeGroup.RegisterGroup("TerraLeague:Tier2Bar", Tier2Bar);

            RecipeGroup Tier3Bar = new RecipeGroup(() => "Adamantite or Titanium Bar", new int[]
            {
                ItemID.AdamantiteBar,
                ItemID.TitaniumBar
            });
            RecipeGroup.RegisterGroup("TerraLeague:Tier3Bar", Tier3Bar);

            RecipeGroup EvilPartGroup = new RecipeGroup(() => "Cursed Flames or Ichor", new int[]
            {
                ItemID.CursedFlame,
                ItemID.Ichor
            });
            RecipeGroup.RegisterGroup("TerraLeague:EvilPartGroup", EvilPartGroup);
            #endregion
        }

        public override void AddRecipes()
        {
            for (int i = 0; i < Recipe.numRecipes; i++)
            {
                Recipe recipe = Main.recipe[i];
                // All recipes that require wood will now need 100% more
                if (recipe.HasResult(ItemID.Leather))
                {
                    if (recipe.TryGetIngredient(ItemID.RottenChunk, out Item ingredient))
                    {
                        recipe.RemoveIngredient(ingredient);
                        recipe.AddRecipeGroup("TerraLeague:EvilDropGroup", 3);
                    }
                }
            }

            CreateRecipe(ItemID.SharkToothNecklace)
                .AddIngredient(ItemID.SharkFin, 4)
                .AddIngredient(ItemID.Chain, 4)
                .AddIngredient(ItemID.SoulofNight, 8)
                .AddTile(TileID.MythrilAnvil)
                .Register();

            CreateRecipe(ItemID.SharkToothNecklace)
                .AddIngredient(ItemID.Stinger, 5)
                .AddIngredient(ItemID.Leather, 2)
                .AddTile(TileID.MythrilAnvil)
                .Register();

            CreateRecipe(ItemID.PirateShirt)
                .AddIngredient(ModContent.ItemType<BrassBar>(), 10)
                .AddIngredient(ItemID.Silk, 4)
                .AddTile(TileID.Anvils)
                .Register();

            CreateRecipe(ItemID.PirateHat)
                .AddIngredient(ModContent.ItemType<BrassBar>(), 16)
                .AddIngredient(ItemID.Silk, 10)
                .AddTile(TileID.Anvils)
                .Register();

            CreateRecipe(ItemID.PiratePants)
                .AddIngredient(ModContent.ItemType<BrassBar>(), 12)
                .AddIngredient(ItemID.Silk, 6)
                .AddTile(TileID.Anvils)
                .Register();

            CreateRecipe(ItemID.BuccaneerBandana)
                .AddIngredient(ModContent.ItemType<BrassBar>(), 10)
                .AddIngredient(ItemID.HellstoneBar, 10)
                .AddTile(TileID.MythrilAnvil)
                .Register();

            CreateRecipe(ItemID.BuccaneerShirt)
                .AddIngredient(ModContent.ItemType<BrassBar>(), 16)
                .AddIngredient(ItemID.HellstoneBar, 16)
                .AddTile(TileID.MythrilAnvil)
                .Register();

            CreateRecipe(ItemID.BuccaneerPants)
                .AddIngredient(ModContent.ItemType<BrassBar>(), 12)
                .AddIngredient(ItemID.HellstoneBar, 12)
                .AddTile(TileID.MythrilAnvil)
                .Register();

            CreateRecipe(ItemID.CobaltShield)
            .AddRecipeGroup("TerraLeague:Tier1Bar", 10)
            .AddIngredient(ItemID.SoulofLight, 10)
            .AddTile(TileID.Anvils)
            .Register();

            CreateRecipe(ItemID.WandofSparking)
            .AddRecipeGroup("Wood", 10)
            .AddIngredient(ItemID.FallenStar, 1)
            .AddIngredient(ItemID.Fireblossom, 1)
            .AddTile(TileID.Anvils)
            .Register();

            CreateRecipe(ItemID.Muramasa)
            .AddIngredient(ModContent.ItemType<ManaBar>(), 18)
            .AddIngredient(ItemID.Bone, 50)
            .AddTile(TileID.Anvils)
            .Register();

            CreateRecipe(ItemID.BandofStarpower)
            .AddIngredient(ItemID.ManaCrystal, 1)
            .AddRecipeGroup("IronBar", 10)
            .AddIngredient(ModContent.ItemType<ManaBar>(), 5)
            .AddTile(TileID.Anvils)
            .Register();

            base.AddRecipes();
        }

        /// <summary>
        /// Creates a ring of dust from a specific player/npc
        /// </summary>
        /// <param name="type">Dust ID to be used</param>
        /// <param name="entity">Player/NPC</param>
        /// <param name="color">Dust color</param>
        internal static void DustRing(int type, Entity entity, Color color)
        {
            for (int i = 0; i < 18; i++)
            {
                Vector2 vel = new Vector2(20, 0).RotatedBy(MathHelper.ToRadians(20 * i));

                Dust dust = Dust.NewDustPerfect(entity.Center, type, vel, 0, color);
                dust.noGravity = true;
            }
        }

        internal static void DustBorderRing(int radius, Vector2 center, int dustType, Color color, float scale, bool noLight = true, bool randomDis = true, float spacingScale = 0.2f)
        {
            float dis = randomDis ? Main.rand.NextFloat(360f / (radius * spacingScale)) : 0;
            for (int i = 0; i < (int)(radius * spacingScale) + 1; i++)
            {
                Vector2 pos = new Vector2(radius, 0).RotatedBy(MathHelper.ToRadians(360 * (i / (radius * spacingScale)) + dis)) + center;

                Dust dustR = Dust.NewDustPerfect(pos, dustType, Vector2.Zero, 0, color, scale);
                dustR.noGravity = true;
                dustR.noLight = noLight;
            }
        }

        internal static void DustElipce(float width, float height, float rotation, Vector2 center, int dustType, Color color, float scale, int dustCount = 180, bool noLight = true, float pulseStrength = 0)
        {
            for (int i = 0; i < dustCount; i++)
            {
                float time = MathHelper.TwoPi * i / (dustCount + 1);

                double X = width * Math.Cos(time) * Math.Cos(rotation) - height * Math.Sin(time) * Math.Sin(rotation);
                double Y = width * Math.Cos(time) * Math.Sin(rotation) + height * Math.Sin(time) * Math.Cos(rotation);

                Vector2 pos = new Vector2((float)X, (float)Y) + center;

                Vector2 velocity = new Vector2(pulseStrength, 0).RotatedBy((pos-center).ToRotation());
                velocity.X *= 1.1f;

                Dust dust = Dust.NewDustPerfect(pos, dustType, (center - pos) * pulseStrength, 0, color, scale);
                dust.noGravity = true;
                dust.noLight = noLight;
                dust.velocity.X *= 1.1f;
            }
        }

        internal static void DustLine(Vector2 pointA, Vector2 pointB, int dustType, float dustPerPoint, float scale = 1, Color color = default, bool noLight = true, float xSpeed = 0, float ySpeed = 0)
        {
            Vector2 velocity = new Vector2(xSpeed, ySpeed);
            float xDif = pointB.X - pointA.X;
            float yDif = pointB.Y - pointA.Y;
            int dustCount = (int)(Vector2.Distance(pointA, pointB) * dustPerPoint);

            for (int i = 0; i < dustCount; i++)
            {
                Vector2 position = new Vector2(pointA.X + (xDif * (i / (float)dustCount)), pointA.Y + (yDif * (i / (float)dustCount)));
                float offsetScale = Main.rand.NextFloat(1);
                position += new Vector2(xDif / dustCount * offsetScale, yDif / dustCount * offsetScale);
                Dust dust = Dust.NewDustPerfect(position, dustType, null, 0, color, scale);
                dust.velocity = velocity;
                dust.noGravity = true;
                dust.noLight = noLight;
            }
        }

        /// <summary>
        /// Sends a message to chat if logging is enabled
        /// </summary>
        /// <param name="message">Message to be sent</param>
        /// <param name="color">Color of the text</param>
        internal static void Log(string message, Color color)
        {
            if (canLog)
            {
                if (Main.netMode == NetmodeID.Server)
                {
                    Console.WriteLine(message);
                }
                else
                {
                    color.A = 0;
                    Main.NewText(message, color);
                }
            }
        }

        /// <summary>
        /// Calculates a velocity between position and the mouse
        /// </summary>
        /// <param name="pos">Starting position for velocity</param>
        /// <param name="speed">Magnitude of the velocity</param>
        /// <returns>Altered velocity</returns>
        internal static Vector2 CalcVelocityToMouse(Vector2 pos, float speed)
        {
            double angle = CalcAngle(pos, Main.MouseWorld);

            if (Main.MouseWorld.X < pos.X)
                return new Vector2(-speed, 0).RotatedBy(angle);
            else
                return new Vector2(speed, 0).RotatedBy(angle);
        }

        internal static Vector2 CalcVelocityToPoint(Vector2 pos, Vector2 point, float speed)
        {
            double angle = CalcAngle(pos, point);

            if (point.X < pos.X)
                return new Vector2(-speed, 0).RotatedBy(angle);
            else
                return new Vector2(speed, 0).RotatedBy(angle);
        }

        /// <summary>
        /// Calculates the angle between a center point and a position
        /// </summary>
        /// <param name="center">The origin point (0,0)</param>
        /// <param name="point">The other point</param>
        /// <returns>The angle between the 2 points</returns>
        internal static double CalcAngle(Vector2 center, Vector2 point)
        {
            double xDis = point.X - center.X;
            double yDis = point.Y - center.Y;

            return Math.Atan(yDis / xDis);
        }

        /// <summary>
        /// Find the itemslot an accessory is in
        /// Returns -1 if not equiped
        /// </summary>
        /// <param name="player">Player to check</param>
        /// <param name="accessory">Accessory to look for</param>
        /// <returns></returns>
        internal static int FindAccessorySlotOnPlayer(Player player, ModItem accessory)
        {
            if (accessory.Item.accessory && player.whoAmI == Main.LocalPlayer.whoAmI)
            {
                for (int i = 0; i < 7; i++)
                {
                    if (player.armor[i + 3].type == accessory.Item.type)
                    {
                        return i;
                    }
                }

            }
            return -1;
        }

        /// <summary>
        /// Removes a buff from an NPC for the local client and all other clients connected to the server
        /// </summary>
        /// <param name="buffType">Buff ID</param>
        /// <param name="target">NPC.whoAmI</param>
        internal static void RemoveBuffFromNPC(int buffType, int target)
        {
            NPC npc = Main.npc[target];

            if (npc.FindBuffIndex(buffType) >= 0)
            {
                npc.DelBuff(npc.FindBuffIndex(buffType));

                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    TerraLeagueNPCsGLOBAL.PacketHandler.SendRemoveBuff(-1, Main.myPlayer, buffType, target);
                }
            }
        }

        /// <summary>
        /// Checks if the projectile is one of the vanilla minions. Base Terraia has no damage type associated with them
        /// </summary>
        /// <param name="proj"></param>
        /// <returns></returns>
        internal static bool IsMinionDamage(Projectile proj)
        {
            if (proj.Name == "Imp Fireball"
                || proj.Name == "Baby Slime"
                || proj.Name == "Hornet Stinger"
                || proj.Name == "Baby Spider"
                || proj.Name == "Mini Sharkron"
                || proj.Name == "Pygmy"
                || proj.Name == "UFO Ray"
                || proj.Name == "Mini Retina Laser"
                || proj.Name == "Stardust Cell"
                || proj.Name == "Frost Blast"
                || proj.Name == "Lunar Portal Laser"
                || proj.Name == "Rainbow Explosion"
                || proj.Name == "Lightning Aura"
                || proj.Name == "Ballista"
                || proj.Name == "Explosive Trap"
                || proj.Name == "Flameburst Tower"
                || proj.Name == "Starburst"
                || proj.minion)
                return true;
            else
                return false;
        }

        //Old Method for old Runnans
        internal static bool DoNotCountRangedDamage(Projectile proj)
        {
            if (proj.Name == "Just false this"
                //|| proj.Name == "Hallow Star"
                //|| proj.Name == "Baby Spider"
                //|| proj.Name == "Mini Sharkron"
                //|| proj.Name == "Pygmy"
                //|| proj.Name == "UFO Ray"
                //|| proj.Name == "Mini Retina Laser"
                //|| proj.Name == "Stardust Cell"
                //|| proj.Name == "Frost Blast"
                //|| proj.Name == "Lunar Portal Laser"
                //|| proj.Name == "Rainbow Explosion"
                //|| proj.Name == "Lightning Aura"
                //|| proj.Name == "Ballista"
                //|| proj.Name == "Explosive Trap"
                //|| proj.Name == "Flameburst Tower"
                //|| proj.Name == "Starburst"
                )
                return true;
            else
                return false;
        }

        /// <summary>
        /// Redundent
        /// </summary>
        /// <param name="proj"></param>
        /// <returns></returns>
        internal static bool IsProjectileHoming(Projectile proj)
        {
            if (proj.type == 207 // Chloro Bullet
                || proj.type == 316 // Bat
                || proj.type == 297 // Specter Staff
                || proj.type == 181 // Bee
                || proj.type == 566 // Large Bee
                || proj.type == 189 // Wasp
                || proj.type == 307 // Scourge Corruptor
                || proj.type == 409 // Razorblade Typhoon
                || proj.type == 634 // Nebula Blaze
                || proj.type == 635 // Nebula Blaze EX
                || proj.type == 618 // Vortex Rocket
                || proj.type == 616 // Vortex Rocket again?
                || proj.type == 617 // Nebula Arcanum
                || proj.type == 619 // Nebula Arcanum again?
                || proj.type == 620 // Nebula Arcanum again...
                || proj.type == 659 // Spirit Flame
                )
                return true;
            else
                return false;
        }

        /// <summary>
        /// Some projectiles are actually npcs so you can hit and destroy them. This checks for that
        /// </summary>
        /// <param name="npc"></param>
        /// <returns></returns>
        internal static bool IsEnemyActuallyProj(NPC npc)
        {
            if (npc.type == NPCID.BurningSphere
                || npc.type == NPCID.ChaosBall
                || npc.type == NPCID.WaterSphere
                || npc.type == NPCID.BlazingWheel
                || npc.type == NPCID.VileSpit
                || npc.type == NPCID.DetonatingBubble
                || npc.type == NPCID.ChatteringTeethBomb
                || npc.type == NPCID.MoonLordLeechBlob
                || npc.type == NPCID.SolarFlare
                || npc.type == NPCID.AncientCultistSquidhead
                || npc.type == NPCID.AncientLight
                || npc.type == NPCID.AncientDoom)
            {
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Checks if the requested Projectile is considered a melee swing by the mod
        /// </summary>
        /// <param name="proj"></param>
        /// <returns></returns>
        internal static bool IsProjActuallyMeleeAttack(Projectile proj)
        {
            if (proj.aiStyle == 19 || proj.aiStyle == 20 || proj.aiStyle == 75 || proj.type == ModContent.ProjectileType<DarksteelBattleaxe_Decimate>() || proj.type == ModContent.ProjectileType<DarkinScythe_ReapingSlash>() || proj.type == ModContent.ProjectileType<Severum_Slash>() || proj.type == ModContent.ProjectileType<AtlasGauntlets_Left>() || proj.type == ModContent.ProjectileType<AtlasGauntlets_Right>())
                return true;
            else
                return false;
        }

        /// <summary>
        /// Converts Terrarias Hot Key strings to shorter, cleaner version for interfaces and tooltips
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>

        

        public static int ScaleWithUseTimeMulti(int value, Item item, Player player)
        {
            return (int)Math.Max(value * (2 - item.GetGlobalItem<Items.TerraLeagueITEMGLOBAL>().UseTimeMultiplier(item, player)), 1);
        }

        /// <summary>
        /// Plays a sound of specific pitch and returns the created instance
        /// </summary>
        /// <param name="position"></param>
        /// <param name="soundID"></param>
        /// <param name="style"></param>
        /// <param name="pitch"></param>
        public static SoundEffectInstance PlaySoundWithPitch(Vector2 position, int soundID, int style, float pitch)
        {
            if (pitch > 1)
                pitch = 1;
            else if (pitch < -1)
                pitch = -1;

            var sound = Terraria.Audio.SoundEngine.PlaySound(new LegacySoundStyle(soundID, style), position);
            if (sound != null)
                sound.Pitch = pitch;

            return sound;
        }

        public static void GetTextureIfNull(ref Texture2D texture, string path)
        {
            if (texture == null || texture.Width <= 1)
                texture = ModContent.Request<Texture2D>(path).Value;
        }

        public static void DrawCircle(Vector2 center, float radius, Color color = default)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.FishingLine.Value;
            Rectangle rectangle = texture.Frame(1, 1, 0, 0, 0, 0);
            Vector2 origin = new Vector2((float)(rectangle.Width / 2), 2f);

            int points = 10;
            for (int i = points; i < radius; i++)
            {
                Vector2 point1 = new Vector2(radius, 0);
                Vector2 point2 = point1.RotatedBy(MathHelper.TwoPi / i);

                if (Vector2.Distance(point1, point2) < 16)
                {
                    points = i;
                    break;
                }
            }

            List<Vector2> pointList = new List<Vector2>();
            for (int i = 0; i < points + 1; i++)
            {
                pointList.Add(center + new Vector2(radius, 0).RotatedBy(MathHelper.TwoPi / points * i));
            }

            Vector2 value2 = pointList[0];
            //Main.spriteBatch.Begin();

            for (int i = 0; i < points; i++)
            {
                Vector2 vector = pointList[i];
                Vector2 vector2 = pointList[i + 1] - vector;
                float rotation = vector2.ToRotation() - 1.57079637f;
                rotation -= 0.04f;
                //Microsoft.Xna.Framework.Color Color = Lighting.GetColor(vector.ToTileCoordinates(), color);
                Vector2 scale = new Vector2(1f, (vector2.Length() + 2f) / (float)rectangle.Height);
                Main.spriteBatch.Draw(texture, value2 - Main.screenPosition, rectangle, color, rotation, origin, scale, SpriteEffects.None, 0f);
                value2 += vector2;
                //Vector2 point1 = new Vector2(radius, 0).RotatedBy(MathHelper.TwoPi / points * i);
                //Vector2 point2 = new Vector2(radius, 0).RotatedBy(MathHelper.TwoPi / points * (i + 1));

                //float rotation = point2.ToRotation() - 1.57079637f;
                //Color LightColor = Lighting.GetColor((point1 + center).ToTileCoordinates(), Color.White);
                //Vector2 scale = new Vector2(1f, (point2.Length() + 2f) / (float)rectangle.Height);

                //Main.spriteBatch.Draw(texture, (center + point2) - Main.screenPosition, rectangle, LightColor, rotation, origin, scale, SpriteEffects.None, 0f);


                //Dust.NewDustPerfect((center + point2), DustID.Dirt);
            }
            //Main.spriteBatch.End();
        }

        public static void DrawLine(Vector2 Point1, Vector2 Point2, Color color = default)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.FishingLine.Value;
            Rectangle rectangle = texture.Frame(1, 1, 0, 0, 0, 0);
            Vector2 origin = new Vector2((float)(rectangle.Width / 2), 0);


            List<Vector2> pointList = new List<Vector2>();


            Vector2 value2 = Point1;
            //Main.spriteBatch.Begin();

            Vector2 vector = Point1;
            Vector2 vector2 = Point2 - vector;
            float rotation = vector2.ToRotation() - 1.57079637f;
            //rotation -= 0.04f;
            //Microsoft.Xna.Framework.Color Color = Lighting.GetColor(vector.ToTileCoordinates(), color);
            Vector2 scale = new Vector2(1f, (vector2.Length() + 2f) / (float)rectangle.Height);
            Main.spriteBatch.Draw(texture, value2 - Main.screenPosition, rectangle, color, rotation, origin, scale, SpriteEffects.None, 0f);
        }
    }
}

enum TerraLeagueHandleType : byte
{
    Player
}
