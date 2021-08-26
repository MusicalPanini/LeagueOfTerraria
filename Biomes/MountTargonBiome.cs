using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerraLeague.Common.ModSystems;
using Terraria;
using Terraria.Graphics.Capture;
using Terraria.ModLoader;

namespace TerraLeague.Biomes
{
    public class MountTargonBiome : ModBiome
    {
        //public override ModWaterStyle WaterStyle => ModContent.Find<ModWaterStyle>("ExampleMod/ExampleWaterStyle"); // Sets a water style for when inside this biome
        //public override ModSurfaceBackgroundStyle SurfaceBackgroundStyle => ModContent.Find<ModSurfaceBackgroundStyle>("TerraLeague/SurfaceMarbleBackgroundStyle");
        public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.Normal;

        public override string BestiaryIcon => "Textures/Bestiary/Biomes/Icon_MountTargon";
        public override string BackgroundPath => "Textures/Bestiary/Biomes/Background_MountTargon";
        public override Color? BackgroundColor => default;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mount Targon");
        }

        public override bool IsBiomeActive(Player player)
        {
            bool withinX = Math.Abs((WorldSystem.TargonCenterX * 16) - player.MountedCenter.X) < Main.maxTilesX / 21 * 16;
            bool withinY = Math.Abs((45 * 16) - player.MountedCenter.Y) < Main.worldSurface * 16;

            bool active = withinX && withinY;
            player.GetModPlayer<PLAYERGLOBAL>().zoneTargon = active;
            return active;
        }
    }
}
