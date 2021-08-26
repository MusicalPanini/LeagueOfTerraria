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
    public class TargonPeakBiome : ModBiome
    {
        //public override ModWaterStyle WaterStyle => ModContent.Find<ModWaterStyle>("ExampleMod/ExampleWaterStyle"); // Sets a water style for when inside this biome
        //public override ModSurfaceBackgroundStyle SurfaceBackgroundStyle => ModContent.Find<ModSurfaceBackgroundStyle>("TerraLeague/SurfaceMarbleBackgroundStyle");
        //public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.ma;

        public override string BestiaryIcon => "Textures/Bestiary/Biomes/Icon_TargonPeak";
        public override string BackgroundPath => "Textures/Bestiary/Biomes/Background_TargonPeak";
        public override Color? BackgroundColor => new Color(43, 247, 165);

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mount Targon's Peak");
        }

        public override bool IsBiomeActive(Player player)
        {
            int sigilID = ModContent.NPCType<NPCs.TargonSigil>();
            if (NPC.CountNPCS(sigilID) != 0)
            {
                Vector2 targonPeak = new Vector2(WorldSystem.TargonCenterX * 16, 45 * 16);

                bool active = targonPeak.Distance(player.MountedCenter) <= Main.worldSurface * 0.3 * 16;

                player.GetModPlayer<PLAYERGLOBAL>().zoneTargonPeak = active;
                player.ManageSpecialBiomeVisuals("TerraLeague:Targon", active);
                return active;
            }

            return false;
        }

        //public override int Music => MusicID.Eerie;
        //public override SceneEffectPriority Priority => SceneEffectPriority.Environment;
        //public override void BiomeVisuals(Player player)
        //{
        //    player.ManageSpecialBiomeVisuals("TerraLeague:Targon", player.GetModPlayer<PLAYERGLOBAL>().zoneTargonPeak);
            
        //    base.BiomeVisuals(player);
        //}
    }
}
