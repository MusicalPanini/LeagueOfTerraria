using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerraLeague.Common.ModSystems;
using Terraria;
using Terraria.Graphics.Capture;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Biomes
{
    public class SurfaceMarbleBiome : ModBiome
    {
        //public override ModWaterStyle WaterStyle => ModContent.Find<ModWaterStyle>("ExampleMod/ExampleWaterStyle"); // Sets a water style for when inside this biome
        public override ModSurfaceBackgroundStyle SurfaceBackgroundStyle => ModContent.Find<ModSurfaceBackgroundStyle>("TerraLeague/SurfaceMarbleBackgroundStyle");
        //public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.ma;

        public override string BestiaryIcon => "Textures/Bestiary/Biomes/Icon_SurfaceMarble";
        public override string BackgroundPath => "Textures/Bestiary/Biomes/Background_SurfaceMarble";
        public override Color? BackgroundColor => new Color(239, 231, 216);

        public override int Music => MusicID.OtherworldlyOcean;
        public override SceneEffectPriority Priority => SceneEffectPriority.BiomeMedium;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Petricte Forest");
        }

        public override bool IsBiomeActive(Player player)
        {
            bool b1 = WorldSystem.marbleBlocks >= 300;

            bool b3 = player.ZoneSkyHeight || player.ZoneOverworldHeight;

            bool active = b1 && b3;
            player.GetModPlayer<PLAYERGLOBAL>().zoneSurfaceMarble = active;
            //nPCSpawnInfo.marble = true;
            return active;
        }
    }
}
