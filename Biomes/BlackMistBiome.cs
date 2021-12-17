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
    public class BlackMistBiome : ModBiome
    {
        //public override ModWaterStyle WaterStyle => ModContent.Find<ModWaterStyle>("ExampleMod/ExampleWaterStyle"); // Sets a water style for when inside this biome
        //public override ModSurfaceBackgroundStyle SurfaceBackgroundStyle => ModContent.Find<ModSurfaceBackgroundStyle>("TerraLeague/SurfaceMarbleBackgroundStyle");
        //public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.ma;

        public override string BestiaryIcon => "Textures/Bestiary/Biomes/Icon_BlackMist";
        //public override string BackgroundPath => "Textures/Bestiary/Biomes/Background_BlackMist";
        public override Color? BackgroundColor => new Color(43, 247, 165);
        public override int Music => MusicID.OtherworldlyNight;
        public override SceneEffectPriority Priority => SceneEffectPriority.Event;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Black Mist");
        }

        public override bool IsBiomeActive(Player player)
        {
            bool onBeachDuringNewMoon = player.ZoneBeach && !Main.dayTime && Main.moonPhase == 4;
            bool onOverworldDuringEvent = WorldSystem.BlackMistEvent && player.ZoneOverworldHeight;

            bool active = onOverworldDuringEvent || onBeachDuringNewMoon;

            player.GetModPlayer<PLAYERGLOBAL>().zoneBlackMist = active;
            player.ManageSpecialBiomeVisuals("TerraLeague:TheBlackMist", active);
            player.blind = true;
            return active;
        }

        //public override int Music => MusicID.Eerie;
        //public override SceneEffectPriority Priority => SceneEffectPriority.Environment;
        //public override void BiomeVisuals(Player player)
        //{
        //    //player.ManageSpecialBiomeVisuals("TerraLeague:TheBlackMist", player.GetModPlayer<PLAYERGLOBAL>().zoneBlackMist);
        //    //player.blind = true;
        //}
    }
}
