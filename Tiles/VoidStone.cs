using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using TerraLeague.Items.Placeable;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;

namespace TerraLeague.Tiles
{
    public class VoidStone : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true; 
            Main.tileLighted[Type] = true; 
            Main.tileBlockLight[Type] = true;
            Main.tileBlendAll[Type] = true;
            Main.tileMerge[Type][TileType<VoidFragment>()] = true;
            Main.tileMerge[TileType<VoidFragment>()][Type] = true;

            SoundType = SoundID.Tink;
            DustType = DustID.Stone;
            ItemDrop = ItemType<TargonStoneBlock>(); 
            AddMapEntry(new Color(75, 0, 75)); 
            MinPick = 65; 
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
        }
    }
}