using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ObjectData;
using TerraLeague.Items;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;

namespace TerraLeague.Tiles
{
    public class PetriciteBarTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileSolidTop[Type] = true;
            Main.tileFrameImportant[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.addTile(Type);

            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Petricite Slab");
            AddMapEntry(new Color(200, 200, 200), name);

            DustType = DustID.Cloud;
        }

        public override bool Drop(int i, int j)
        {
            Tile t = Main.tile[i, j];
            int style = t.TileFrameX / 18;
            if (style == 0) 
            {
                Item.NewItem(i * 16, j * 16, 16, 16, ItemType<Petricite>());
            }
            return base.Drop(i, j);
        }
    }
}