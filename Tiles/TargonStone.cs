using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using TerraLeague.Items.Placeable;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;

namespace TerraLeague.Tiles
{
    public class TargonStone : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true; 
            Main.tileLighted[Type] = true; 
            Main.tileBlockLight[Type] = true;
            Main.tileBlendAll[Type] = true;

            SoundType = SoundID.Tink;
            DustType = DustID.Stone;
            ItemDrop = ItemType<TargonStoneBlock>(); 
            AddMapEntry(new Color(172, 154, 138)); 
            MinPick = 100; 
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
        }

        public override bool CanExplode(int i, int j)
        {
            return false;
        }

        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
        {
            return Common.ModSystems.DownedBossSystem.downedTargonBoss && Main.hardMode;
        }
    }
}