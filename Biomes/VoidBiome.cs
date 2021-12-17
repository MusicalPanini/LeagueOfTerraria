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
    public class VoidBiome : ModBiome
    {
        //public override ModWaterStyle WaterStyle => ModContent.Find<ModWaterStyle>("ExampleMod/ExampleWaterStyle"); // Sets a water style for when inside this biome
        //public override ModUndergroundBackgroundStyle UndergroundBackgroundStyle => base.UndergroundBackgroundStyle;
        //public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.;

        public override string BestiaryIcon => "Textures/Bestiary/Biomes/Icon_Void";
        public override string BackgroundPath => "Textures/Bestiary/Biomes/Background_Void";
        public override Color? BackgroundColor => base.BackgroundColor;
        public override int Music => Terraria.ID.MusicID.OtherworldlyUGCorrption;


        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Void Warped Caves");
        }

        public override SceneEffectPriority Priority => SceneEffectPriority.BiomeHigh;

        public override bool IsBiomeActive(Player player)
        {
            bool active = WorldSystem.voidBlocks >= 300;

            player.GetModPlayer<PLAYERGLOBAL>().zoneVoid = active;
            return active;
        }
    }
}
